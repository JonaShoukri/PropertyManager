//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PRMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PropertyManager
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyManager()
        {
            this.Properties = new HashSet<Property>();
        }
    
        public int PropertyManagerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int OwnerId { get; set; }
        public string Password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Property> Properties { get; set; }
        public virtual PropertyOwner PropertyOwner { get; set; }
    }
}
