using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

using System.Xml;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class SettingDAL
    {
        #region stored Procedures

        private const string spSetting_Get = "dbo.Setting_Get"; 
        private const string spSetting_Update = "dbo.Setting_Update"; 
        private const string spSetting_Create = "dbo.Setting_Create";

        #endregion

		private static readonly SettingConfiguration[] settingConfigurations = new SettingConfiguration[]{
															 new SettingConfiguration("ActivitiesMaxSec", "ActivitiesMaxSec", typeof(int), 60),
															 new SettingConfiguration("ShutDownCountdownSec", "ShutDownCountdownSec", typeof(int), 20),
                                                             new SettingConfiguration("PaymentTargetVisible", "PaymentTargetVisible", typeof(Boolean), false),
                                                             new SettingConfiguration("AddressSafeTime","AddressSafeTime",  typeof(int), 180),
                                                             new SettingConfiguration("AddressSafeActiv","AddressSafeActiv", typeof(Boolean),false),
                                                             new SettingConfiguration("AbortDialingTime","AbortDialingTime", typeof(int), 30),
                                                             new SettingConfiguration("DomainEmailLogin", "DomainEmailLogin", typeof(string), "metatop"), 
														 };
		
        private class SettingConfiguration
		{
			public readonly string SettingKey;
			public readonly string PropertyName;
			public readonly Type TypeOfSetting;
			public readonly object DefaultValue;

			public SettingConfiguration(string settingKey, string propertyName, Type typeOfSetting, object defaultValue)
			{
				this.SettingKey = settingKey;
				this.PropertyName = propertyName;
				this.TypeOfSetting = typeOfSetting;
				this.DefaultValue =  defaultValue;
			}
		}

		/// <summary>
		/// ruft die aktuellen Anwendungseinstellungen von der Datenbank ab
		/// </summary>
		/// <returns></returns>
		public static Setting GetSettings()
		{
			Setting setting = new Setting();
			Type settingType = setting.GetType();

            DataTable settingDataTable = SqlHelper.ExecuteDataTable(spSetting_Get);

			if (settingDataTable.Rows.Count < settingConfigurations.Length)
				CreateSettings();

			foreach(SettingConfiguration configuration in settingConfigurations)
			{
				System.Reflection.PropertyInfo property =  settingType.GetProperty(configuration.PropertyName, configuration.TypeOfSetting);
				if (property == null)
				{
					throw new InvalidOperationException(string.Format("Property {0} not found in type {1}", configuration.PropertyName, settingType.Name));
				}
				
				settingDataTable.PrimaryKey = new DataColumn[]{ settingDataTable.Columns["SettingName"]};
				DataRow row =  settingDataTable.Rows.Find(configuration.SettingKey);
				if (row == null)
				{
					//throw new InvalidOperationException(string.Format("Setting {0} not defined", configuration.SettingKey));
					property.SetValue(setting, Convert.ChangeType(configuration.DefaultValue, property.PropertyType), null);
				}
				else
				{
					
					object value = row["SettingValue"];
					//Bei Null-Values wird der Defaultwert belassen
					if ((value != null) && (value != DBNull.Value))
						property.SetValue(setting, Convert.ChangeType(value, property.PropertyType), null);
				}
			}

			return setting;
		}

		/// <summary>
		/// aktualisiert alle Anwendungseinstellungen auf der Datenbank
		/// </summary>
		/// <param name="setting"></param>
		public static void UpdateSettings(Setting setting)
		{
			Type settingType = setting.GetType();
			
            IDictionary<string, object> parameters = new Dictionary<string, object>();

			parameters.Add("@SettingName", null	);
			parameters.Add("@SettingValue", null);

			foreach(SettingConfiguration configuration in settingConfigurations)
			{
				System.Reflection.PropertyInfo property =  settingType.GetProperty(configuration.PropertyName, configuration.TypeOfSetting);
				if (property == null)
				{
					throw new InvalidOperationException(string.Format("Property {0} not found in type {1}", configuration.PropertyName, settingType.Name));
				}

				parameters["@SettingName"] = configuration.SettingKey;
				parameters["@SettingValue"] = property.GetValue(setting, null);

                SqlHelper.ExecuteStoredProc(spSetting_Update,  parameters);
			}
		}

	
		private static void CreateSettings()
		{
            IDictionary<string, object> parameters = new Dictionary<string, object>();

			parameters.Add("@SettingName", null);
			parameters.Add("@DefaultValue", null);
			parameters.Add("@ValueType", null);

			foreach(SettingConfiguration configuration in settingConfigurations)
			{
				parameters["@SettingName"] = configuration.SettingKey;
				parameters["@DefaultValue"] = configuration.DefaultValue;
				parameters["@ValueType"] = configuration.TypeOfSetting.Name;

                SqlHelper.ExecuteStoredProc(spSetting_Create,parameters);
			}
		}
    }
}
