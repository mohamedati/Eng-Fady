  
namespace ERP.Models
{
    public static class clsEnum
    {
        public enum Roles
        {

            مدير_النظام,
            أعضاء_هيئة_التدريس,
            موظف,
    
        }
       
        public enum VacationRequestStatus
        {
            جديد = 1,
            مرفوض = 2,
            مقبول = 3,
            ملغى=4

        }
        public enum VacationTypes
        {
            أجازة_اعتيادية ,
            أجازة_مرضية,
            أجازة_مرافقة_مريض ,
            أجازة_وفاة ,
            أجازة_الأبوية,
        }  public enum HospitalTypes
        {
            خاص ,
            حكومى,

        }
    

    }
}
