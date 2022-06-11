// See https://aka.ms/new-console-template for more information
using DemoGA;

/*
 * 
DIMENSION 1 || DIMENSION 2
Thứ     || Buổi SÁNG - Tiết
=========================================================
Thứ 2   || Tiết 1 [0,0] || Tiết 2 [0,1] || Tiết 3 [0,2] || Tiết 4 [0,3] || Tiết 5 [0,4]
Thứ 3   || Tiết 1 [1,0] || Tiết 2 [1,1] || Tiết 3 [1,2] || Tiết 4 [1,3] || Tiết 5 [1,4]
Thứ 4   || Tiết 1 [2,0] || Tiết 2 [2,1] || Tiết 3 [2,2] || Tiết 4 [2,3] || Tiết 5 [2,4]
Thứ 5   || Tiết 1 [3,0] || Tiết 2 [3,1] || Tiết 3 [3,2] || Tiết 4 [3,3] || Tiết 5 [3,4]
Thứ 6   || Tiết 1 [4,0] || Tiết 2 [4,1] || Tiết 3 [4,2] || Tiết 4 [4,3] || Tiết 5 [4,4]
Thứ 7   || Tiết 1 [5,0] || Tiết 2 [5,1] || Tiết 3 [5,2] || Tiết 4 [5,3] || Tiết 5 [5,4]  
=========================================================
Thứ     || Buổi CHIỀU - Tiết
=========================================================
Thứ 2   || Tiết 1 [6,0] || Tiết 2 [6,1] || Tiết 3 [6,2] || Tiết 4 [6,3] || Tiết 5 [6,4]
Thứ 3   || Tiết 1 [7,0] || Tiết 2 [7,1] || Tiết 3 [7,2] || Tiết 4 [7,3] || Tiết 5 [7,4]
Thứ 4   || Tiết 1 [8,0] || Tiết 2 [8,1] || Tiết 3 [8,2] || Tiết 4 [8,3] || Tiết 5 [8,4]
Thứ 5   || Tiết 1 [9,0] || Tiết 2 [9,1] || Tiết 3 [9,2] || Tiết 4 [9,3] || Tiết 5 [9,4]
Thứ 6   || Tiết 1 [10,0] || Tiết 2 [10,1] || Tiết 3 [10,2] || Tiết 4 [10,3] || Tiết 5 [10,4]
Thứ 7   || Tiết 1 [11,0] || Tiết 2 [11,1] || Tiết 3 [11,2] || Tiết 4 [11,3] || Tiết 5 [11,4]  
 */

int n_iter = 1000;
int n_pop = 200;
double r_cross = 0.9;
double r_mut = 0.05;

GradeInfo gradeInfo = new GradeInfo();

gradeInfo.Id = 11;
gradeInfo.Name = "Khối 11";

List<TeacherInfo> teachers = InitData.GetListTeacher();

gradeInfo.Classes = InitData.GetListClass(teachers);

List<TeacherAssignedLessonsInfo> teacherAssignedLessons = new List<TeacherAssignedLessonsInfo>();
List<Timetable> listTimetable = new List<Timetable>();

for (int i = 0; i < gradeInfo.Classes.Count; i++)
{
    Timetable timetable = new Timetable(gradeInfo.Classes[i]);

    Functions.GeneticAlgorithm2(n_iter, n_pop, r_cross, r_mut, ref timetable, ref teacherAssignedLessons);

    listTimetable.Add(timetable);
}

Console.ReadLine();