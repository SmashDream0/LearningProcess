using Dapper.Contrib.Extensions;
using LearningProcess.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Entity
{
    [Table("Material")]
    public class Material: IEntity
    {
        [Column("ML_Key")]
        [Key]
        public int Key { get; set; }

        [Column("ML_Name")]
        [DataLength(55)]
        public string Name { get; set; }

        [Column("ML_Body")]
        [DataLength(50*1024*1024)]
        public byte[] Body { get; set; }

        [Column("ML_FileName")]
        [DataLength(255)]
        public string FileName { get; set; }

        [Column("ML_MaterialTypeKey")]
        public int MaterialTypeKey { get; set; }
    }
}
