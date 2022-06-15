using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGA
{
    public static class InitData
    {
        #region ========== INPUTS ==========
        public const string MORNING_SECTION = "morning";
        public const string AFTERNOON_SECTION = "afternoon";

        public const int WEEKDAY = 6;
        public const int LESSONSPERSECTION = 5;

        public const string PRIMARY_SECTION = "primary_section";
        public const string SECONDARY_SECTION = "secondary_section";

        public static List<TeacherInfo> GetListTeacher()
        {
            List<TeacherInfo> result = new List<TeacherInfo>();

            result.AddRange(new List<TeacherInfo>
            {
                new TeacherInfo(1, "BGH"),
                new TeacherInfo(2, "Hạnh"),
                new TeacherInfo(3, "Thư"),
                new TeacherInfo(4, "Huế"),
                new TeacherInfo(5, "Minh"),
                new TeacherInfo(6, "Hiền"),
                new TeacherInfo(7, "Lan"),
                new TeacherInfo(8, "Thư"),
                new TeacherInfo(9, "Quý"),
                new TeacherInfo(10, "Linh"),
                new TeacherInfo(11, "Xuân"),
                new TeacherInfo(12, "Trang"),
                new TeacherInfo(13, "Hải"),
                new TeacherInfo(14, "Tú Anh"),
                new TeacherInfo(15, "Thúy"),
                new TeacherInfo(16, "Nguyên"),
                new TeacherInfo(17, "Mai"),
                new TeacherInfo(18, "Liễu"),
                new TeacherInfo(19, "Gấm"),
                new TeacherInfo(20, "Trọng"),
                new TeacherInfo(21, "Kiên"),
                new TeacherInfo(22, "Khuyên"),
                new TeacherInfo(23, "Dũng"),
                new TeacherInfo(24, "Q.Trang"),
                new TeacherInfo(25, "Phượng"),
                new TeacherInfo(26, "Lộc"),
                new TeacherInfo(27, "Thòn"),
                new TeacherInfo(28, "Tuấn"),
                new TeacherInfo(29, "Liên"),
                new TeacherInfo(30, "Phước"),
                new TeacherInfo(31, "Tuyên"),
                new TeacherInfo(32, "Ly"),
                new TeacherInfo(33, "Thạnh"),
            });

            return result;
        }

        public static List<SubjectInfo> GetListSubject(string section)
        {
            List<SubjectInfo> result = new List<SubjectInfo>();

            result.AddRange(new List<SubjectInfo> {
                new SubjectInfo(1, "SHDC", 1, 1, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(2, "SHCN", 1, 1, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(3, "Sử", 2, 2, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(4, "Hóa", 3, 2, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(5, "Lý", 3, 2, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(6, "Văn", 5, 3, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(7, "Toán", 5, 3, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(8, "Anh", 2, 2, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(9, "Sinh", 2, 2, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(10, "Địa", 1, 1, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(11, "Tin", 1, 1, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(12, "GDCD", 1, 1, new List<string>(), PRIMARY_SECTION),
                new SubjectInfo(13, "Thể dục", 2, 2, new List<string>(), SECONDARY_SECTION),
                new SubjectInfo(14, "GDQP", 2, 2, new List<string>(), SECONDARY_SECTION),
            }
            );

            // Hardcode, nếu chính khóa là buổi sáng thì SHDC, SHCN là tiết 1, tiết 2 Thứ 2
            if (section == MORNING_SECTION)
            {
                result[0].FixedLessons.Add("0_0");
                result[1].FixedLessons.Add("0_1");
            }
            else if (section == AFTERNOON_SECTION) // Hardcode, nếu chính khóa là buổi chiều thì SHDC, SHCN là tiết 4, tiết 5 Thứ 6
            {
                result[0].FixedLessons.Add("4_4");
                result[1].FixedLessons.Add("4_3");
            }

            return result;
        }

        public static List<ClassInfo> GetListClass(List<TeacherInfo> teachers)
        {
            List<ClassInfo> result = new List<ClassInfo>();

            #region Lớp 11A1
            ClassInfo c = new ClassInfo();
            c.Id = 1;
            c.Name = "11A1";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[3];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A2
            c = new ClassInfo();
            c.Id = 2;
            c.Name = "11A2";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[12];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A3
            c = new ClassInfo();
            c.Id = 3;
            c.Name = "11A3";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[10];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A4
            c = new ClassInfo();
            c.Id = 4;
            c.Name = "11A4";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[21];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A5
            c = new ClassInfo();
            c.Id = 5;
            c.Name = "11A5";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[9];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A6
            c = new ClassInfo();
            c.Id = 6;
            c.Name = "11A6";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[13];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A7
            c = new ClassInfo();
            c.Id = 7;
            c.Name = "11A7";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[22];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A8
            c = new ClassInfo();
            c.Id = 8;
            c.Name = "11A8";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[28];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A9
            c = new ClassInfo();
            c.Id = 9;
            c.Name = "11A9";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[23];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A10
            c = new ClassInfo();
            c.Id = 10;
            c.Name = "11A10";
            c.MainSection = AFTERNOON_SECTION;
            c.HeadTeacher = teachers[32];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A11
            c = new ClassInfo();
            c.Id = 11;
            c.Name = "11A11";
            c.MainSection = AFTERNOON_SECTION;
            c.HeadTeacher = teachers[27];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A12
            c = new ClassInfo();
            c.Id = 12;
            c.Name = "11A12";
            c.MainSection = AFTERNOON_SECTION;
            c.HeadTeacher = teachers[29];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A13
            c = new ClassInfo();
            c.Id = 13;
            c.Name = "11A13";
            c.MainSection = AFTERNOON_SECTION;
            c.HeadTeacher = teachers[24];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion


            #region Lớp 11A14
            c = new ClassInfo();
            c.Id = 14;
            c.Name = "11A14";
            c.MainSection = AFTERNOON_SECTION;
            c.HeadTeacher = teachers[29];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            #region Lớp 11A15
            c = new ClassInfo();
            c.Id = 15;
            c.Name = "11A15";
            c.MainSection = AFTERNOON_SECTION;
            c.HeadTeacher = teachers[24];
            c.Subjects = GetListSubject(c.MainSection);

            result.Add(c);
            #endregion

            return result;
        }

        // Lấy danh sách các môn học xếp tuyến tính vào TKB
        public static Lessons[,] GetInputLessons(List<SubjectInfo> subjects, List<TeacherInfo> teachers, List<TeachingDistribution> teachingDistributions)
        {
            Lessons[,] result = new Lessons[WEEKDAY, LESSONSPERSECTION];
            int row = 0, column = 0;

            int index = 1;

            for (int i = 0; i < subjects.Count; i++)
            {
                if (row == WEEKDAY) break;
                TeachingDistribution? td = teachingDistributions.Where(x => x.SubjectId.Any(y => y == subjects[i].Id)).FirstOrDefault();

                if (td != null)
                {
                    TeacherInfo? teacher = teachers.Find(x => x.Id == td.TeacherId);

                    if (teacher != null)
                    {
                        for (int j = 0; j < subjects[i].LessonsPerWeek; j++)
                        {
                            string address = row.ToString() + "_" + column.ToString();

                            if (column == LESSONSPERSECTION)
                            {
                                row++;
                                column = 0;
                            }
                            Lessons lessons = new Lessons();
                            lessons.Id = index;
                            lessons.Subject = subjects[i];
                            lessons.Teacher = teacher;

                            if (subjects[i].FixedLessons.Contains(address)) lessons.IsLock = 1;

                            result[row, column] = lessons;
                            column++;

                            index++;
                        }
                    }
                }
            }

            return result;
        }

        // Phân công giảng dạy
        public static List<TeachingDistribution> GetTeachingDistributions()
        {
            List<TeachingDistribution> result = new List<TeachingDistribution>();

            TeachingDistribution td = new TeachingDistribution();
            List<int> classesId = new List<int>();

            // Teacher 1
            td.TeacherId = 1;
            td.SubjectId = new List<int>();
            td.SubjectId.AddRange(new List<int> { 1, 2 });
            classesId.AddRange(new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            });
            td.ClassessId = classesId;
            result.Add(td);

            // Teacher 2
            td = new TeachingDistribution();
            td.TeacherId = 2;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(7);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 5, 9, 13
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 3
            td = new TeachingDistribution();
            td.TeacherId = 3;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(7);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                2, 6, 10, 14
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 4
            td = new TeachingDistribution();
            td.TeacherId = 4;
            td.SubjectId = new List<int>();
            td.SubjectId.Add(7);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                3, 7, 11, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 5
            td = new TeachingDistribution();
            td.TeacherId = 5;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(7);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                4, 8, 12
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 6
            td = new TeachingDistribution();
            td.TeacherId = 6;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(6);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                 1, 5, 9 , 13
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 7
            td = new TeachingDistribution();
            td.TeacherId = 7;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(6);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                2, 6, 10, 14
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 8
            td = new TeachingDistribution();
            td.TeacherId = 8;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(6);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                3, 7, 11, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 9
            td = new TeachingDistribution();
            td.TeacherId = 9;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(6);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                4, 8, 12
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 10
            td = new TeachingDistribution();
            td.TeacherId = 10;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(5);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 4, 7, 10, 13
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 11
            td = new TeachingDistribution();
            td.TeacherId = 11;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(5);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                2, 5, 8, 11, 14
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 13
            td = new TeachingDistribution();
            td.TeacherId = 13;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(5);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                3, 6, 9 , 12, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 14
            td = new TeachingDistribution();
            td.TeacherId = 14;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(4);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 4, 7, 10, 13
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 15
            td = new TeachingDistribution();
            td.TeacherId = 15;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(4);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                2, 5, 8, 11, 14
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 16
            td = new TeachingDistribution();
            td.TeacherId = 16;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(4);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                3, 6, 9, 12, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 17
            td = new TeachingDistribution();
            td.TeacherId = 17;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(9);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 3, 5, 7, 9, 11, 13, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 18
            td = new TeachingDistribution();
            td.TeacherId = 18;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(9);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                2, 4, 6, 8, 10, 12, 14
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 19
            td = new TeachingDistribution();
            td.TeacherId = 19;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(3);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                2, 4, 6, 8, 10, 12, 14
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 22
            td = new TeachingDistribution();
            td.TeacherId = 22;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(3);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 3, 5, 7, 9, 11, 13, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 23
            td = new TeachingDistribution();
            td.TeacherId = 23;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(8);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 2, 5, 6, 9, 10, 13, 14
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 24
            td = new TeachingDistribution();
            td.TeacherId = 24;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(8);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                3, 4, 7, 8, 11, 12, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 25
            td = new TeachingDistribution();
            td.TeacherId = 25;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(12);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            });
            td.ClassessId = classesId;
            result.Add(td);

            // Teacher 26
            td = new TeachingDistribution();
            td.TeacherId = 26;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(10);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 27
            td = new TeachingDistribution();
            td.TeacherId = 27;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(11);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 28
            td = new TeachingDistribution();
            td.TeacherId = 28;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(13);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 2, 3, 4, 5, 6, 7
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 29
            td = new TeachingDistribution();
            td.TeacherId = 29;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(13);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                8, 9, 10, 11, 12, 13, 14, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            // Teacher 30
            td = new TeachingDistribution();
            td.TeacherId = 30;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(14);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8
            });
            td.ClassessId = classesId;
            result.Add(td);

            // Teacher 31
            td = new TeachingDistribution();
            td.TeacherId = 31;

            td.SubjectId = new List<int>();
            td.SubjectId.Add(14);

            classesId = new List<int>();
            classesId.AddRange(new List<int>
            {
               9, 10, 11, 12, 13, 14, 15
            });
            td.ClassessId = classesId;

            result.Add(td);

            return result;
        }

        // Xáo trộn các tiết trong 1 TKB => làm đầu vào để chạy thuật toán
        public static T[,] Shuffle<T>(Random random, T[,] array)
        {
            int lengthRow = array.GetLength(1);

            for (int i = array.Length - 1; i > 0; i--)
            {
                int i0 = i / lengthRow;
                int i1 = i % lengthRow;

                int j = random.Next(i + 1);
                int j0 = j / lengthRow;
                int j1 = j % lengthRow;

                T temp = array[i0, i1];
                array[i0, i1] = array[j0, j1];
                array[j0, j1] = temp;
            }

            return array;
        }
        #endregion
    }
}
