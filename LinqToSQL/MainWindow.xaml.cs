using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace LinqToSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        LinqToSqlDataClassesDataContext dataContext;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSQL.Properties.Settings.gqdbConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);

            //InsertUniversities();
            //InsertStudents();
            //InsertLectures();
            //InsertStudentLectureAssociations();
            //GetUniversityOfToni();
            GetLecturesOfToni();
        }

        // Add a University
        public void InsertUniversities()
        {
            dataContext.ExecuteCommand("delete from University");

            University yale = new University(); // New instance of University
            yale.Name = "Yale"; // Set name property of new instance
            dataContext.Universities.InsertOnSubmit(yale); // Insert data into collection on submit

            University harvard = new University();
            harvard.Name = "Harvard";
            dataContext.Universities.InsertOnSubmit(harvard);

            dataContext.SubmitChanges(); // Submit

            MainDataGrid.ItemsSource = dataContext.Universities; // Set source of data for MainDataGrid element to Universities collection
        }

        // Add Students
        public void InsertStudents()
        {
            dataContext.ExecuteCommand("delete from Student");


            University yale = dataContext.Universities.First(un => un.Name.Equals("Yale")); // First document in University collection which has the name Yale. Return as object
            University harvard = dataContext.Universities.First(un => un.Name.Equals("Harvard"));

            List<Student> students = new List<Student>(); // Initialize a new list of Students

            students.Add(new Student { Name = "Carla", Gender = "female", UniversityId = yale.Id }); // Add using university Id
            students.Add(new Student { Name = "Toni", Gender = "male", University = yale }); // Add using university object
            students.Add(new Student { Name = "Leyla", Gender = "female", University = harvard });
            students.Add(new Student { Name = "James", Gender = "trans-gender", University = harvard });

            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLectures()
        {
            dataContext.ExecuteCommand("delete from Lecture");


            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "History" });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations()
        {
            Student Carla = dataContext.Students.First(st => st.Name.Equals("Carla"));
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));
            Student Leyla = dataContext.Students.First(st => st.Name.Equals("Leyla"));
            Student James = dataContext.Students.First(st => st.Name.Equals("James"));

            Lecture Math = dataContext.Lectures.First(lc => lc.Name.Equals("Math"));
            Lecture History = dataContext.Lectures.First(lc => lc.Name.Equals("History"));

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Carla, Lecture = Math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Toni, Lecture = Math });

            StudentLecture slToni = new StudentLecture();
            slToni.Student = Toni;
            slToni.LectureId = History.Id;
            dataContext.StudentLectures.InsertOnSubmit(slToni);

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Leyla, Lecture = History });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.StudentLectures;
        }

        public void GetUniversityOfToni()
        {
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));

            University TonisUniversity = Toni.University;

            List<University> universities = new List<University>();
            universities.Add(TonisUniversity);

            MainDataGrid.ItemsSource = universities;
        }

        public void GetLecturesOfToni()
        {
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));

            var tonisLectures = from sl in Toni.StudentLectures select sl.Lecture;

            MainDataGrid.ItemsSource = tonisLectures;
        }


    }
}
