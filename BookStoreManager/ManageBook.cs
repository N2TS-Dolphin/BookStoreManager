using BookStoreManager.Database;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Microsoft.Build.Evaluation;
using System.Net.NetworkInformation;
using EnvDTE;
using EnvDTE80;
using Project = EnvDTE.Project;
using ProjectItem = EnvDTE.ProjectItem;
using Microsoft.VisualStudio.Shell;

namespace BookStoreManager
{
    class ManageBook
    {
        public static BindingList<CategoryModel> GetCategories()
        {
            return CategoryDao.getCategoryList();
        }

        public static int AddNewBook(BookModel newBook)
        {
            return BookDao.InsertNewBookToDB(newBook);
        }

        //public static string SaveImageToProject(string filePath, string bookid)
        //{
        //    //string projectFilePath = "C:\\Users\\DELL\\source\\repos\\N2TS-Dolphin\\BookStoreManager\\BookStoreManager\\BookStoreManager.csproj";
        //    string newFileName = Path.GetFileName(filePath);
        //    string projectDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName).FullName;

        //    string imageFolderPath = Path.Combine(projectDirectory, "Image");
        //    //imageFolderPath.Replace('/', '\\');

        //    newFileName = $"{bookid}_BookIMG{Path.GetExtension(filePath)}";
        //    string newFilePath = Path.Combine(imageFolderPath, newFileName);
        //    if (!File.Exists(newFilePath))
        //    {
        //        File.Copy(filePath, newFilePath);
        //        var projectFilePath = GetProjectFilePath();
        //        MessageBox.Show($"111{projectFilePath}");
        //        SetBuildActionToResource(projectFilePath, newFilePath);
        //    }
        //    return newFileName;
        //}
        //public static bool DeleteImageFromProject(string filename)
        //{
        //    string binDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //    string projectDirectory = Directory.GetParent(Directory.GetParent(binDirectory).FullName).FullName;
        //    string imagePathToDelete = Path.Combine(projectDirectory, "Image", filename);
        //    imagePathToDelete.Replace('/', '\\');
        //    MessageBox.Show(imagePathToDelete);
        //    if (File.Exists(imagePathToDelete))
        //    {
        //        File.Delete(imagePathToDelete);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Xóa hình thất bại");
        //        return false;
        //    }
        //    return true;
        //}

        //private static void SetBuildActionToResource(string projectFilePath, string filePath)
        //{
        //    // Lấy một thể hiện của DTE từ gói dịch vụ của Visual Studio
        //    DTE2 dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

        //    if (dte == null)
        //    {
        //        // Xử lý khi không thể truy cập DTE
        //        return;
        //    }

        //    // Lấy dự án .csproj từ đối tượng DTE
        //    Project project = dte.Solution.FindProjectItem(projectFilePath)?.Object as Project;
        //    if (project == null)
        //    {
        //        // Xử lý nếu không tìm thấy dự án
        //        return;
        //    }

        //    // Tìm tệp trong dự án
        //    ProjectItem item = null;
        //    foreach (ProjectItem projectItem in project.ProjectItems)
        //    {
        //        if (projectItem.FileNames[1] == filePath)
        //        {
        //            item = projectItem;
        //            break;
        //        }
        //    }

        //    // Nếu tệp được tìm thấy trong dự án
        //    if (item != null)
        //    {
        //        // Thiết lập Build Action của tệp thành "EmbeddedResource"
        //        item.Properties.Item("ItemType").Value = "Resource"; // Thiết lập loại tệp thành EmbeddedResource

        //        // Lưu thay đổi
        //        project.Save();
        //    }
        //}
        //public static string GetProjectFilePath()
        //{
        //    // Lấy đường dẫn tới thư mục làm việc hiện tại
        //    string currentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName).FullName;
        //    MessageBox.Show($"111{currentDirectory}");
        //    // Tìm tệp dự án trong thư mục làm việc hiện tại hoặc các thư mục cha
        //    string projectFilePath = $"{currentDirectory}\\BookStoreManager.csproj";
        //    MessageBox.Show($"111{projectFilePath}");

        //    return projectFilePath;
        //}
        ////public static string GetProjectFilePath()
        ////{
        ////    // Load dự án .csproj hoặc .vbproj vào MSBuild project
        ////    Project msbuildProject = ProjectCollection.GlobalProjectCollection.GetLoadedProjects("*.csproj").FirstOrDefault();

        ////    if (msbuildProject != null)
        ////    {
        ////        // Lấy đường dẫn tới tệp dự án
        ////        string projectFilePath = msbuildProject.FullPath;

        ////        return projectFilePath;
        ////    }

        ////    return null;
        ////}
    }
}
