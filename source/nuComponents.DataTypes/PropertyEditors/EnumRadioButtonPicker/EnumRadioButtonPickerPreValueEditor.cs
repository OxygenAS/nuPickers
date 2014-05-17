﻿
namespace nuComponents.DataTypes.PropertyEditors.EnumRadioButtonPicker
{
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class EnumRadioButtonPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "Label Macro", "App_Plugins/nuComponents/DataTypes/Shared/CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("radioButtonPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/RadioButtonPicker/RadioButtonPickerConfig.html", HideLabel = true)]
        public string RadioButtonPicker { get; set; }

        [PreValueField("relationTypeMapping", "", "App_Plugins/nuComponents/DataTypes/Shared/RelationTypeMapping/RelationTypeMappingConfig.html", HideLabel = true)]
        public string RelationTypeMapping { get; set; }

        [PreValueField("saveFormat", "Save Format", "App_Plugins/nuComponents/DataTypes/Shared/SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }

        [PreValueField("apiController", "EnumRadioButtonPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
