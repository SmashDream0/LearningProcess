using LearningProcess.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.MVVM.ViewModel
{
    public class ViewModelSettings
    {
        public ViewModelSettings()
        {
            AllowDoubleClick = true;
            AllowSingleClick = false;
            IsReadOnly = Program.SettingsInstance.IsReadOnly;
        }

        /// <summary>
        /// Разрешить двойной клик
        /// </summary>
        public bool AllowDoubleClick
        { get; set; }

        /// <summary>
        /// Разрешить единичный клик
        /// </summary>
        public bool AllowSingleClick
        { get; set; }

        /// <summary>
        /// Разрешение на редактирование
        /// </summary>
        public bool IsReadOnly
        { get; set; }

        /// <summary>
        /// Двойной клик по внутреннему элементу
        /// </summary>
        public Action<IEntity> ElementDoubleClick
        { get; set; }

        /// <summary>
        /// Единственный клик по внутреннему элементу
        /// </summary>
        public Action<IEntity> ElementClick
        { get; set; }
    }
}
