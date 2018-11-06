using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CollecumentData;
using DocxSearcher;

namespace CollecumentBl
{
    public class FileManager
    {
        public static FileStream OpenFile(string fileName, string extention)
        {
            string filePath = @"E:\Documents\GitHub\ColleCument\files";
            //שליפה מהדטה בייס כדי לפרק את התאריך כדי לדעת את מיקום הקובץ
            CollecumentData.File file = GetFileById(fileName);

            filePath += "\\" + file.Date_Creation.Year.ToString();
            filePath += "\\" + file.Date_Creation.Month.ToString();
            filePath += "\\" + file.Date_Creation.Day.ToString();
            fileName += '.' + GetExtentionById(extention);
            filePath += "\\" + fileName;
            FileStream files = System.IO.File.Open(filePath, FileMode.Open);
            return files;
        }

        public static void SetLastEntryInDb(string Tz)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                User user = Logic.GetUserByTz(Tz);
                string date = DateTime.Today.ToString("yyyy-MM-dd");
                //לא עובד!!!!!!
                //context.Users.FirstOrDefault(x => x.TZ == Tz).LastEntry = DateTime.Parse(date);

            };

        }

        public static List<Template> GetAllTemplates()
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Templates.ToList();
            }
        }

        private static string GetExtentionById(string extention)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                var exten = context.Extensions.FirstOrDefault(x => x.ID.ToString() == extention);
                if (exten != null)
                {
                    return exten.Extension1;
                }
                return null;
            }
        }

        public static void SaveNewTemplateInDB(string fileid, string description, string extension, string CreatorName)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {

                Template template = new Template();
                template.ID = fileid;
                template.Name = description;
                template.ExtensionId = GetExtentionByName(extension);
                context.Templates.Add(template);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            };
        }

        public static void SaveNewFileInDB(string fileid, string selectCategory, string description, string CreatorName, string Extension, string Remark)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                CollecumentData.File file = new CollecumentData.File();
                file.ID = fileid;
                file.Description = description;
                file.CreatorID = Logic.GetUserByName(CreatorName);
                file.Date_Creation = DateTime.Now;
                file.ExtensionID = GetExtentionByName(Extension);
                file.Version = 1;
                file.CategoryID = GetCategoryById(selectCategory);
                file.Remark = Remark;
                context.Files.Add(file);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            };
        }

        private static int GetExtentionByName(string Extension)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                var exten = context.Extensions.FirstOrDefault(x => x.Extension1 == Extension);
                if (exten != null)
                {
                    return exten.ID;
                }
                return 0;
            }
        }

        private static int GetCategoryById(string selectCategory)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                var cat = context.Categories.FirstOrDefault(x => x.Description == selectCategory);
                if (cat != null)
                {
                    return cat.ID;
                }
                return 0;
            }
        }

        private static CollecumentData.File GetFileById(string fileName)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Files.FirstOrDefault(x => x.ID == fileName);
            }
        }
        public static List<CollecumentData.File> GetAllFiles()
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Files.ToList();
            }
        }

        public static List<CollecumentData.File> GetAllFilesAccordingUser()
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                //איך לשלוף מהסשיין ???????
                string tZ = "206741936";
                User user = Logic.GetUserByTz(tZ);
                List<CollecumentData.File> file = context.Files.ToList();
                List<CollecumentData.File> Allfile = new List<CollecumentData.File>();
                List<Sharing> sharings = context.Sharings.Where(x => x.TZ == tZ).ToList();
                foreach (var sharing in sharings)
                {
                    Allfile.Add(context.Files.FirstOrDefault(x => x.ID == sharing.FileID));
                }
                return Allfile.ToList();
            }
        }

        public static List<Extension> GetAllExtensions()
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Extensions.ToList();
            }
        }

        public static FileStream CreateFile(out string idFile, string extension)
        {

            string path = @"E:\Documents\GitHub\ColleCument\files\";
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            string[] arrdate = date.Split('-');
            foreach (string time in arrdate)
            {
                path += time + @"\";
            }
            DirectoryInfo dic = Directory.CreateDirectory(path);
            Guid guid = Guid.NewGuid();
            idFile = guid.ToString();
            path += guid + "." + extension;
            FileStream file = System.IO.File.Create(path);
            return file;
        }
        public static FileStream CreateTemplate(out string idFile, string extension)
        {
            string path = @"E:\Documents\GitHub\ColleCument\files\templates\";
            DirectoryInfo dic = Directory.CreateDirectory(path);
            Guid guid = Guid.NewGuid();
            idFile = guid.ToString();
            path += guid + "." + extension;
            FileStream file = System.IO.File.Create(path);
            return file;
        }
        public static List<CollecumentData.File> SearchTextInFile(string keyword)
        {
            //FileStream inFile = new FileStream(@"E:\Documents\Hello.docx", FileMode.Open, FileAccess.Read);

            string directory = @"E:\Documents\GitHub\ColleCument\files";
            string searchString = keyword;
            bool searchSubdirectories = true;
            bool caseSensitive = true;
            bool useRegex = false;
            List<CollecumentData.File> searchfiles = new List<CollecumentData.File>();
            string docxText = null;
            var isMatch = useRegex ? new Predicate<string>(x => Regex.IsMatch(x, searchString, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase))
                    : new Predicate<string>(x => x.IndexOf(searchString, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var filePath in Directory.GetFiles(directory, "*.docx", searchSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (stream.Length == 0)
                        continue;
                    docxText = new DocxToStringConverter(stream).Convert();

                    {
                        if (isMatch(docxText))
                        {
                            using (DBCollecumentEntities context = new DBCollecumentEntities())
                            {
                                string idFile = stream.Name.Substring(stream.Name.Length - 41).ToString();
                                string id = idFile.Remove(idFile.Length - 5);
                                CollecumentData.File file = context.Files.FirstOrDefault(x => x.ID == id);
                                searchfiles.Add(file);
                            }

                        }
                    }
                }
            }
                return searchfiles;

        }
    }
}
