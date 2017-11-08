using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class DataFieldManager
    {
        private static System.Collections.Generic.Dictionary<string, MethodInfo> dataFieldMethods = null;

        public static List<string> AvailableDataFields
        {
            get
            {
                if (dataFieldMethods == null)
                {
                    dataFieldMethods = GetDataFieldMethods();
                }

                return new List<string>(dataFieldMethods.Keys);
            }
        }

        public static MethodInfo GetMethod(string dataField)
        {

            if (dataFieldMethods == null)
            {
                dataFieldMethods = GetDataFieldMethods();
            }


            if (!dataFieldMethods.ContainsKey(dataField))
                throw new DataFieldMethodNotFoundException();



            return dataFieldMethods[dataField];
        }

        public static DataFieldMethodCategory GetCategory(string dataField)
        {
            MethodInfo method = GetMethod(dataField);

            return GetCategory(method);
        }

        public static DataFieldMethodCategory GetCategory(MethodInfo dataFieldMethod)
        {

            DataFieldMethodAttribute attribute = GetDataFieldMethodAttribute(dataFieldMethod);

            if (attribute != null)
                return attribute.Category;
            else
                throw new InvalidOperationException("no DataFieldMethodAttribute present");
        }

        private static DataFieldMethodAttribute GetDataFieldMethodAttribute(MethodInfo dataFieldMethod)
        {
            object[] attributes = dataFieldMethod.GetCustomAttributes(typeof(DataFieldMethodAttribute), true);
            if (attributes.Length == 0)
                return null;

            return attributes[0] as DataFieldMethodAttribute;
        }

        private static Dictionary<string, MethodInfo> GetDataFieldMethods()
        {
            Dictionary<string, MethodInfo> dataFieldMethods = new Dictionary<string, MethodInfo>();

            Assembly asm = Assembly.GetExecutingAssembly();

            Type[] exportedTypes = asm.GetTypes();

            // Einsammeln der Info's
            foreach (Type type in exportedTypes)
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

                foreach (MethodInfo method in methods)
                {
                    Attribute[] attributes = method.GetCustomAttributes(typeof(DataFieldMethodAttribute), false) as Attribute[];

                    if (attributes != null &&
                        attributes.Length > 0)
                    {

                        DataFieldMethodAttribute dataFieldMethodAttribute = attributes[0] as DataFieldMethodAttribute;

                        if (dataFieldMethodAttribute != null)
                        {
                            dataFieldMethods.Add(dataFieldMethodAttribute.DataField, method);
                        }
                    }
                }
            }

            return dataFieldMethods;
        }

        public virtual object GetValue(string dataField)
        {
            MethodInfo method = GetMethod(dataField);

            if (method == null)
                return null;

            return GetValue(method, null);
        }

        public virtual object GetValue(string dataField, DataFieldMethodParameter param)
        {
            MethodInfo method = GetMethod(dataField);

            return GetValue(method, param);
        }

        public virtual object GetValue(MethodInfo dataFieldMethod, DataFieldMethodParameter param)
        {
            if (dataFieldMethod == null)
                throw new ArgumentNullException("dataFieldMethod");

            //Parameterprüfung
            if (param == null ||
                param.Parameters.Count < 1)
            {
                throw new ArgumentException("Es wurde keine oder falsche Parameter übergeben.");
            }

            return dataFieldMethod.Invoke(null, new object[] { param });
        }

        public virtual object GetValue(MethodInfo dataFieldMethod)
        {
            return GetValue(dataFieldMethod, null);
        }

    }

}
