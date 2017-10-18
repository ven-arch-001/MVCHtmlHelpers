using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System;
using System.Text.RegularExpressions;

namespace Mvc.Web.Test.Filters
{
    /// <summary>
    ///     Meta data definition for oData output to include pagination
    /// </summary>
    /// <typeparam name="T">Type of the entity returned</typeparam>
    public class ODataResult<T> where T : class
    {
        private readonly long? _count;
        private IEnumerable<T> _result;

        public ODataResult(IEnumerable<T> result, long? count)
        {
            _count = count;
            
            List<T> execResult = result.ToList();

            //Custom manipulation -- Load from cache
            execResult.ForEach(t => { });
            _result = execResult;
            // _result = result;
        }

        public IEnumerable<T> Results
        {
            get { return _result; }
        }

        public long? Count
        {
            get { return _count; }
        }
    }

    public interface RecordCountItem
    {
        int RecordCount { get; set; }
    }
     

    /// <summary>
    /// http://www.strathweb.com/2012/08/supporting-odata-inlinecount-with-the-new-web-api-odata-preview-package/
    /// </summary>
    public class CustomQueryableAttribute : EnableQueryAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                long? originalsize = null;
                var inlinecount = HttpUtility.ParseQueryString(actionExecutedContext.Request.RequestUri.Query).Get("$inlinecount");


                //Change the default filter
                if (actionExecutedContext.Request.Properties.ContainsKey("$includeObsolete"))
                {
                   
                }



                // this._unitOfWorkAsync.Context.ChangeFilter(Framework.Repository.Pattern.Infrastructure.Filters.Obsolete, true);


                object responseObject;
                actionExecutedContext.Response.TryGetContentValue(out responseObject);
                var originalquery = responseObject as IQueryable<object>;

                //if (originalquery != null && inlinecount == "allpages")
                //    originalsize = originalquery.LongCount();

                base.OnActionExecuted(actionExecutedContext);

                if (ResponseIsValid(actionExecutedContext.Response))
                {
                    actionExecutedContext.Response.TryGetContentValue(out responseObject);
                    if (responseObject is IQueryable)
                    {
                        var robj = responseObject as IQueryable<object>;
                        //Fetch the record set
                        IEnumerable<object> data = robj.ToList();

                        if (originalquery != null && inlinecount == "allpages")
                        {
                            string originalUri = actionExecutedContext.Request.RequestUri.AbsoluteUri;
                            originalUri = HttpUtility.UrlDecode(originalUri);

                            //Remove the $top and $skip parameters from URL to fetch the total count
                            Regex reg = new Regex(@"&\$top(=[^&]*)?|^\$top(=[^&]*)?&?");
                            originalUri = reg.Replace(originalUri, string.Empty);

                            reg = new Regex(@"&\$skip(=[^&]*)?|^\$skip(=[^&]*)?&?");
                            originalUri = reg.Replace(originalUri, string.Empty);

                            //Assign the new request URI
                            actionExecutedContext.Request.RequestUri = new Uri(originalUri);
                            
                            var newODataOption = new ODataQueryOptions(queryOptionsInternal.Context, actionExecutedContext.Request);

                            //New IQuery Instance created with new oDATA options (to exclude $top and $skip)
                            var couQuery = this.ApplyQuery(originalquery, newODataOption);
                            
                            var couQueryCasted = couQuery as IQueryable<object>;
                            //Find the total count
                            originalsize = couQueryCasted.LongCount(); 
                        }
                        else if (originalquery != null)
                            originalsize = robj.LongCount();

                        if (originalsize != null)
                        { 
                            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK,
                               new ODataResult<object>(data, originalsize));
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Custom manipulation of the filter
                if (actionExecutedContext.Request.Properties.ContainsKey("$includeObsolete"))
                {
                    
                }
            }
        }

        public override void ValidateQuery(HttpRequestMessage request, ODataQueryOptions queryOptions)
        {
            //everything is allowed
        }

        ODataQueryOptions queryOptionsInternal = null;
        public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            queryOptionsInternal = queryOptions;
            if (queryOptions.Filter != null)
            {
                //Incase of custom filtering
            }

            return queryOptions.ApplyTo(queryable);
        }
        private bool ResponseIsValid(HttpResponseMessage response)
        {
            if (response == null || response.StatusCode != HttpStatusCode.OK || !(response.Content is ObjectContent)) return false;
            return true;
        }
    }
}