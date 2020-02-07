using Dapper.Contrib.Extensions;
using LearningProcess.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Entity
{
    [Table("MaterialType")]
    public class MaterialType : IEntity
    {
        [Column("MT_Key")]
        [Key]
        public int Key { get; set; }

        [Column("MT_Name")]
        [DataLength(55)]
        public string Name { get; set; }

        [Column("MT_DisciplineKey")]
        public int DisciplineKey { get; set; }
    }
}
