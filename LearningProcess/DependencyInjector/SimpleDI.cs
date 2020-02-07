using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.DependencyInjector
{
    public class SimpleDI : IBinderTo
    {
        public SimpleDI()
        {
            _settings = new Dictionary<Type, BindSettings>();
        }

        private BindSettings _newBindSetting;

        private readonly Dictionary<Type, BindSettings> _settings;

        /// <summary>
        /// Привязать один тип к другому
        /// </summary>
        /// <typeparam name="T">Типа привязки от</typeparam>
        /// <returns></returns>
        public IBinderTo Bind<T>()
        {
            var type = typeof(T);

            if (_settings.ContainsKey(type))
            { throw new Exception($"Type {type} allready presented in bindings"); }

            _newBindSetting = new BindSettings();
            _newBindSetting.TFrom = typeof(T);

            return this;
        }

        /// <summary>
        /// Привязать один тип к другому
        /// </summary>
        /// <typeparam name="T">Тип привязки к</typeparam>
        /// <param name="isSingleInstance">Использовать единственный экземпляр</param>
        /// <returns></returns>
        public SimpleDI To<T>(bool isSingleInstance)
        {
            _newBindSetting.TTo = typeof(T);
            _newBindSetting.IsSingleInstance = isSingleInstance;

            _settings.Add(_newBindSetting.TFrom, _newBindSetting);

            _newBindSetting = null;

            return this;
        }

        public SimpleDI To(object instance)
        {
            _newBindSetting.TTo = null;
            _newBindSetting.IsSingleInstance = true;
            _newBindSetting.Instance = instance;

            _settings.Add(_newBindSetting.TFrom, _newBindSetting);

            _newBindSetting = null;

            return this;
        }

        /// <summary>
        /// Получить связанный экземпляр типа TBind
        /// </summary>
        /// <typeparam name="TBind">Тип привязки</typeparam>
        /// <param name="parameters">Параметры конструктора</param>
        /// <returns></returns>
        public object GetInstance<TBind>(params object[] parameters)
        {
            return GetInstance(typeof(TBind), parameters);
        }

        private object GetInstance(Type tBind, params object[] parameters)
        {
            if (!_settings.ContainsKey(tBind))
            { throw new Exception($"Can't find binded type {tBind}"); }

            var settings = _settings[tBind];

            if (!settings.IsSingleInstance || settings.Instance == null)
            {
                if (parameters == null || parameters.Length == 0)
                {
                    //Если входящих параметров конструктора нет
                    //, то пробую их получить из списка параметров имеющего конструктора типа settings.TTo
                    parameters = GetBindedParameters(settings.TTo);
                }

                var result = Activator.CreateInstance(settings.TTo, parameters);

                if (settings.IsSingleInstance)
                { settings.Instance = result; }

                return result;
            }
            else
            { return settings.Instance; }
        }

        private object[] GetBindedParameters(Type type)
        {
            var constructors = type.GetConstructors();

            //Если есть конструктор без параметров, то использую его
            if (constructors.FirstOrDefault(x => x.GetParameters().Length == 0) != null)
            { return new object[0]; }

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                //Если есть тип параметра, который не представлен в списках привязок, то этот конструктор не подходит в принципе
                if (parameters.FirstOrDefault(x => !_settings.ContainsKey(x.ParameterType)) == null)
                {
                    var instances = constructor.GetParameters().Select(x => GetInstance(x.ParameterType));

                    return instances.ToArray();
                }
            }

            //Если дошел до сюда, то нужный конструктор не найден
            throw new Exception($"Can't find binded arguments for type {type}");
        }
    }
}
