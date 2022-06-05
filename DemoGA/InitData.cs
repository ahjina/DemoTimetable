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
        const string MORNING_SECTION = "morning";
        const string AFTERNOON_SECTION = "afternoon";

        public static List<TeacherInfo> GetListTeacher()
        {
            List<TeacherInfo> result = new List<TeacherInfo>();

            result.AddRange(new List<TeacherInfo>
            {
                new TeacherInfo(1, "Ban giám hiệu"),
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
                new TeacherInfo(12, "Trang", 1),
                new TeacherInfo(13, "Hải"),
                new TeacherInfo(14, "Tú Anh"),
                new TeacherInfo(15, "Thúy"),
                new TeacherInfo(16, "Nguyên"),
                new TeacherInfo(17, "Mai"),
                new TeacherInfo(18, "Liễu"),
                new TeacherInfo(19, "Gấm"),
                new TeacherInfo(20, "Trọng", 1),
                new TeacherInfo(21, "Kiên", 1),
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

        public static List<SubjectInfo> GetListSubject()
        {
            List<SubjectInfo> result = new List<SubjectInfo>();

            result.AddRange(new List<SubjectInfo> {
                new SubjectInfo(1, "SHDC", 1, 1, null, null, null),
                new SubjectInfo(2, "SHCN", 1, 1, null, null, null),
                new SubjectInfo(3, "Sử", 2, 2, null, null, null),
                new SubjectInfo(4, "Hóa", 4, 2, null, null, null),
                new SubjectInfo(5, "Lý", 4, 2, null, null, null),
                new SubjectInfo(6, "Văn", 5, 3, null, null, null),
                new SubjectInfo(7, "Toán", 6, 3, null, null, null),
                new SubjectInfo(8, "Anh", 3, 2, null, null, null),
                new SubjectInfo(9, "Sinh", 2, 2, null, null, null),
                new SubjectInfo(10, "Địa", 2, 2, null, null, null),
            }
            );

            return result;
        }

        public static List<ClassInfo> GetListClass(List<TeacherInfo> teachers, List<SubjectInfo> subjects)
        {
            List<ClassInfo> result = new List<ClassInfo>();
            List<ClassSubjectInfo> classSubjectInfos = new List<ClassSubjectInfo>();

            List<TeacherInfo> tempTeachers = new List<TeacherInfo>();
            List<ClassSubjectInfo> tempCsi = new List<ClassSubjectInfo>();

            #region Lớp 11A1
            ClassInfo c = new ClassInfo();
            c.Id = 1;
            c.Name = "11A1";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[3];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[1], teachers[2], teachers[3], teachers[4], teachers[5], teachers[6], teachers[7], teachers[8]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A2
            c = new ClassInfo();
            c.Id = 2;
            c.Name = "11A2";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[12];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[1], teachers[2], teachers[3], teachers[12], teachers[5], teachers[6], teachers[7], teachers[8]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A3
            c = new ClassInfo();
            c.Id = 3;
            c.Name = "11A3";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[10];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[9], teachers[10], teachers[3], teachers[21], teachers[13], teachers[14], teachers[7], teachers[8]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A4
            c = new ClassInfo();
            c.Id = 4;
            c.Name = "11A4";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[21];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[9], teachers[10], teachers[3], teachers[21], teachers[22], teachers[14], teachers[15], teachers[16]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A5
            c = new ClassInfo();
            c.Id = 5;
            c.Name = "11A5";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[9];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[9], teachers[10], teachers[3], teachers[21], teachers[13], teachers[6], teachers[15], teachers[16]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A6
            c = new ClassInfo();
            c.Id = 6;
            c.Name = "11A6";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[13];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[9], teachers[18], teachers[26], teachers[21], teachers[22], teachers[6], teachers[15], teachers[16]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A7
            c = new ClassInfo();
            c.Id = 7;
            c.Name = "11A7";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[22];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[9], teachers[25], teachers[26], teachers[27], teachers[22], teachers[6], teachers[23], teachers[16]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A8
            c = new ClassInfo();
            c.Id = 8;
            c.Name = "11A8";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[28];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[9], teachers[25], teachers[26], teachers[27], teachers[28], teachers[6], teachers[23], teachers[16]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A9
            c = new ClassInfo();
            c.Id = 9;
            c.Name = "11A9";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[23];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[9], teachers[18], teachers[26], teachers[27], teachers[28], teachers[6], teachers[23], teachers[24]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A10
            c = new ClassInfo();
            c.Id = 10;
            c.Name = "11A10";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[32];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[17], teachers[29], teachers[30], teachers[31], teachers[32], teachers[14], teachers[15], teachers[24]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A11
            c = new ClassInfo();
            c.Id = 11;
            c.Name = "11A11";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[27];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[17], teachers[29], teachers[30], teachers[31], teachers[32], teachers[14], teachers[15], teachers[24]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A12
            c = new ClassInfo();
            c.Id = 12;
            c.Name = "11A2";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[29];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[17], teachers[29], teachers[30], teachers[31], teachers[32], teachers[14], teachers[15], teachers[24]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            #region Lớp 11A13
            c = new ClassInfo();
            c.Id = 13;
            c.Name = "11A13";
            c.MainSection = MORNING_SECTION;
            c.HeadTeacher = teachers[24];

            // Sử - Hóa - Lý - Văn - Toán - Anh - Sinh - Địa
            tempTeachers.AddRange(new List<TeacherInfo>
            {
                teachers[17], teachers[29], teachers[30], teachers[31], teachers[32], teachers[14], teachers[15], teachers[24]
            });

            tempCsi = CreateList(tempTeachers, subjects, teachers[0], c.HeadTeacher);
            classSubjectInfos.AddRange(tempCsi);

            c.Subjects = classSubjectInfos;

            result.Add(c);
            #endregion

            return result;
        }

        private static List<ClassSubjectInfo> CreateList(List<TeacherInfo> teacherIndex, List<SubjectInfo> subjectIndex, TeacherInfo schoolAdmin, TeacherInfo headTeacher)
        {
            List<ClassSubjectInfo> result = new List<ClassSubjectInfo>();

            // SHDC
            ClassSubjectInfo csi = new ClassSubjectInfo();
            csi.TeacherInfo = schoolAdmin;
            csi.SubjectInfo = subjectIndex[0];
            result.Add(csi);

            // SHCN
            csi = new ClassSubjectInfo();
            csi.TeacherInfo = headTeacher;
            csi.SubjectInfo = subjectIndex[1];
            result.Add(csi);

            // 8 subject
            for (int i = 0; i < 8; i++)
            {
                csi = new ClassSubjectInfo();
                csi.TeacherInfo = teacherIndex[i];
                csi.SubjectInfo = subjectIndex[i];

                result.Add(csi);
            }

            return result;
        }
        #endregion
    }
}
