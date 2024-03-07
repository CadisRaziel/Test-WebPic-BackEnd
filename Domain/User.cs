using UserApiWebPic.Enums;

namespace UserApiWebPic.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }       
    }    
}