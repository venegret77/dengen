//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MashZavod.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization.Json;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;

    [DataContract]
    public partial class users
    {
        
        public users()
        {
            this.access = new HashSet<access>();
        }

        [Key]
        [DataMember]
        public int id_users { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Patronymic { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Position { get; set; }
        public Nullable<int> id_rank { get; set; }
        public Nullable<int> id_group_rank { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<access> access { get; set; }
        [DataMember]
        public virtual group_rank group_rank { get; set; }
        [DataMember]
        public virtual rank rank { get; set; }
        [DataMember]
        public virtual User_chat User_chat { get; set; }
    }
}