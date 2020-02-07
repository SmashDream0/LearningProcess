using Dapper.Contrib.Extensions;
using LearningProcess.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Entity
{
    [Table("Discipline")]
    public class Discipline : IEntity
    {
        [Column("DN_Key")]
        [Key]
        public int Key { get; set; }

        [Column("DN_Name")]
        [DataLength(55)]
        public string Name { get; set; }

        [Column("DN_Image")]
        [DataLength(5242880)]
        public byte[] Image { get; set; }
    }
}
