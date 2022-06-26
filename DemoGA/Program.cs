// See https://aka.ms/new-console-template for more information

using DemoGA;
using System.Text;

/* !
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

int n_iter = 2500; // Số lần lặp để tìm ra TKB tốt nhất
int n_pop = 500; // Số lượng Container (chứa TKB) và số lượng TKB trong 1 container => làm input đầu vào
double r_cross = 0.9; // Tỉ lệ phối giống (crossover)
double r_mut = 0.1; // Tỉ lệ đột biến (mutation)

GradeInfo gradeInfo = new GradeInfo();

gradeInfo.Id = 11;
gradeInfo.Name = "Khối 11";

List<TeacherInfo> teachers = InitData.GetListTeacher();

gradeInfo.Classes = InitData.GetListClass(teachers);

List<TeacherAssignedLessonsInfo> teacherAssignedLessons = new List<TeacherAssignedLessonsInfo>(); // Phân công giảng dạy
List<Timetable> listTimetable = new List<Timetable>(); // Danh sách TKB buổi sáng
List<Timetable> listTimetable2 = new List<Timetable>(); // Danh sách TKB buổi chiều

for (int i = 0; i < gradeInfo.Classes.Count; i++)
{
    Console.OutputEncoding = Encoding.UTF8;
    // MORNING - TKB buổi sáng
    Timetable timetable = new Timetable(gradeInfo.Classes[i]);
    timetable.Section = InitData.MORNING_SECTION;

    var tmp = teacherAssignedLessons.ConvertAll(x => new TeacherAssignedLessonsInfo(x));

    // Gọi xử lý thuật toán
    Functions.GeneticAlgorithm2(n_iter, n_pop, r_cross, r_mut, ref timetable, ref tmp);

    listTimetable.Add(timetable);

    // Sử dụng kết quả xếp tiết của TKB mới nhất để làm rule mới cho TKB sau
    if (i == 0) teacherAssignedLessons = tmp; // nếu là TKB đầu tiên => lấy kết quả xếp tiết làm rule cho TKB sau
    else // Ngược lại, merge kết quả xếp tiết hiện tại và kết quả xếp tiết của TKB mới nhất làm rule cho TKB kế tiếp
    {
        for (int j = 0; j < tmp.Count; j++)
        {
            var index = teacherAssignedLessons.FindIndex(x => x.TeacherId == tmp[j].TeacherId);

            if (index < 0) teacherAssignedLessons.Add(tmp[j]);
            else
            {
                for (int l = 0; l < tmp[j].AssignedLessonInfos.Count; l++)
                {
                    var tmpTAL = teacherAssignedLessons[index].AssignedLessonInfos.Find(x => x.Address == tmp[j].AssignedLessonInfos[l].Address);

                    if (tmpTAL == null)
                    {
                        teacherAssignedLessons[index].AssignedLessonInfos.Add(new AssignedLessonInfo(tmp[j].AssignedLessonInfos[l].Address, tmp[j].AssignedLessonInfos[l].ClassId, tmp[j].AssignedLessonInfos[l].ClassName, tmp[j].AssignedLessonInfos[l].Section));
                    }
                }
            }
        }
    }

    // AFTERNOON - TKB buổi chiều
    Timetable timetable2 = new Timetable(gradeInfo.Classes[i]);
    timetable2.Section = InitData.AFTERNOON_SECTION;

    var tmp2 = teacherAssignedLessons.ConvertAll(x => new TeacherAssignedLessonsInfo(x));

    Functions.GeneticAlgorithm2(n_iter, n_pop, r_cross, r_mut, ref timetable2, ref tmp);

    listTimetable2.Add(timetable2);

    if (i == 0) teacherAssignedLessons = tmp;
    else
    {
        for (int j = 0; j < tmp.Count; j++)
        {
            var index = teacherAssignedLessons.FindIndex(x => x.TeacherId == tmp[j].TeacherId);

            if (index < 0) teacherAssignedLessons.Add(tmp[j]);
            else
            {
                for (int l = 0; l < tmp[j].AssignedLessonInfos.Count; l++)
                {
                    var tmpTAL = teacherAssignedLessons[index].AssignedLessonInfos.Find(x => x.Address == tmp[j].AssignedLessonInfos[l].Address);

                    if (tmpTAL == null)
                    {
                        teacherAssignedLessons[index].AssignedLessonInfos.Add(new AssignedLessonInfo(tmp[j].AssignedLessonInfos[l].Address, tmp[j].AssignedLessonInfos[l].ClassId, tmp[j].AssignedLessonInfos[l].ClassName, tmp[j].AssignedLessonInfos[l].Section));
                    }
                }
            }
        }
    }
}

Console.ReadLine();