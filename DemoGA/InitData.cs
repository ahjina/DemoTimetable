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
                new SubjectInfo(1, "SHCN", 1, 1, null, null, null),
                new SubjectInfo(1, "Sử", 2, 2, null, null, null),
                new SubjectInfo(1, "Hóa", 4, 2, null, null, null),
                new SubjectInfo(1, "Lý", 4, 2, null, null, null),
                new SubjectInfo(1, "Văn", 5, 3, null, null, null),
                new SubjectInfo(1, "Toán", 6, 3, null, null, null),
                new SubjectInfo(1, "Anh", 3, 2, null, null, null),
                new SubjectInfo(1, "Sinh", 2, 2, null, null, null),
                new SubjectInfo(1, "Địa", 2, 2, null, null, null),
            }
            );

            return result;
        }

        public static List<ClassInfo> GetListClass()
        {
            List<ClassInfo> result = new List<ClassInfo>();

            return result;
        }
        #endregion
    }
}
