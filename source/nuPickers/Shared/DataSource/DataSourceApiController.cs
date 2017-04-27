﻿namespace nuPickers.Shared.DataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.Editor;
    using System.Linq;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using DotNetDataSource = DotNetDataSource.DotNetDataSource;
    using EnumDataSource = EnumDataSource.EnumDataSource;
    using JsonDataSource = JsonDataSource.JsonDataSource;
    using LuceneDataSource = LuceneDataSource.LuceneDataSource;
    using RelationDataSource = RelationDataSource.RelationDataSource;
    using SqlDataSource = SqlDataSource.SqlDataSource;
    using XmlDataSource = XmlDataSource.XmlDataSource;

    [PluginController("nuPickers")]
    public class DataSourceApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// This method will return 'data editor items' for a given property editor
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="parentId"></param>
        /// <param name="propertyAlias"></param>
        /// <param name="data"></param>
        /// <returns>a <see cref="GetEditorDataItemsResponse"/> object</returns>
        [HttpPost]
        public GetEditorDataItemsResponse GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            // build return object
            var response = new GetEditorDataItemsResponse()
            {
                EditorDataItems = Enumerable.Empty<EditorDataItem>(),
                Count = null
            };

            IDataSource dataSource = null;

            // NOTE: the value of 'apiController' was previously used in the js to identify an api controller to call for a specific data-source.
            // the js now always calls this api contoller, so the 'apiController' value has been re-purposed here to identify the data-source type to de-serialize
            // changing this var name would be a breaking change, as all data-types would need to be resaved (so rename in v2.0.0)
            switch ((string)data.config.dataSource.apiController)
            {
                case "DotNetDataSourceApi": dataSource = ((JObject)data.config.dataSource).ToObject<DotNetDataSource>(); break;
                case "EnumDataSourceApi": dataSource = ((JObject)data.config.dataSource).ToObject<EnumDataSource>(); break;
                case "JsonDataSourceApi": dataSource = ((JObject)data.config.dataSource).ToObject<JsonDataSource>(); break;
                case "LuceneDataSourceApi": dataSource = ((JObject)data.config.dataSource).ToObject<LuceneDataSource>(); break;
                case "RelationDataSourceApi": dataSource = ((JObject)data.config.dataSource).ToObject<RelationDataSource>(); break;
                case "SqlDataSourceApi": dataSource = ((JObject)data.config.dataSource).ToObject<SqlDataSource>(); break;
                case "XmlDataSourceApi": dataSource = ((JObject)data.config.dataSource).ToObject<XmlDataSource>(); break;
            }

            if (dataSource != null)
            {
                // TODO: log error if more than one param (typeahead, keys, page) supplied


                // typeahead
                if (data.typeahead != null)
                {
                    response.EditorDataItems = Editor.GetEditorDataItems(
                                                        currentId,
                                                        parentId,
                                                        propertyAlias,
                                                        dataSource,
                                                        (string)data.config.customLabel,
                                                        (string)data.typeahead);

                    // response.Count not set, as full collection size not known
                }
                // keys
                else if (data.keys != null)
                {
                    response.EditorDataItems = Editor.GetEditorDataItems(
                                                        currentId,
                                                        parentId,
                                                        propertyAlias,
                                                        dataSource,
                                                        (string)data.config.customLabel,
                                                        ((JArray)data.keys).Select(x => x.ToString()).ToArray());

                    // response.count not set, as request is for specific items
                }
                // page
                else if (data.page != null)
                {
                    int count;

                    response.EditorDataItems = Editor.GetEditorDataItems(
                                                        currentId,
                                                        parentId,
                                                        propertyAlias,
                                                        dataSource,
                                                        (string)data.config.customLabel,
                                                        (int)data.config.pagedListPicker.itemsPerPage,
                                                        (int)data.page, 
                                                        out count);

                    response.Count = count;
                }
                // default
                else
                {
                    response.EditorDataItems = Editor.GetEditorDataItems(
                                                        currentId,
                                                        parentId,
                                                        propertyAlias,
                                                        dataSource,
                                                        (string)data.config.customLabel);

                    // full collection returned
                    response.Count = response.EditorDataItems.Count();
                }
            }

            return response;
        }
    }
}