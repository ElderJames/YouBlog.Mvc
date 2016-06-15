using System;
using System.Linq;
using You.Models;
using You.Core;
using You.Data.Types;
using You.Data;

namespace You.Service
{
    /// <summary>
    /// 公共服务
    /// <remarks>
    /// 创建：2014.02.23
    /// </remarks>
    /// </summary>
    public class CommonModelService: BaseService<CommonModel>
    {
        public IQueryable<CommonModel> FindList(int number, string model, string title, string subTitle, string tag, string keyword, int categoryID, string inputer, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, OrderType orderType)
        {
            return FindPageList(out number, 1, number, model, title, subTitle,tag,keyword, categoryID, inputer, fromDate, toDate, orderType);
        }

        public IQueryable<CommonModel> FindPageList(string tag,out int total, int pageIndex, int pageSize)
        {
            var list = FindPageList(pageIndex, pageSize, out total, cm => cm.Tags.Contains(tag) && cm.State == CommonModelState.Normal && !cm.isPage, OrderType.Desc, cm => cm.ReleaseDate);
            return list;
        }

        public IQueryable<CommonModel> FindPageList(int categoryID,out int total, int pageIndex, int pageSize)
        {
            var list=FindPageList(pageIndex, pageSize,out total,com => (com.CategoryID == categoryID || com.Category.ParentPath.Contains("|" + categoryID + "|")) && com.State == CommonModelState.Normal, OrderType.Desc, cm => cm.ReleaseDate);
            return list;
        }

        public IQueryable<CommonModel> FindPageList(out int totalRecord, int? pageIndex, int? pageSize, string model, string title, string subTitle, string tag,string keyword, int categoryID, string inputer, DateTime? fromDate, DateTime? toDate, OrderType orderType,CommonModelState state=CommonModelState.Normal)
        {
            var whereLandba = PredicateBuilder.True<CommonModel>();
            if (model == null || model != "All") whereLandba = whereLandba.And(cm => cm.Model == model);
            if (!string.IsNullOrEmpty(title)) whereLandba = whereLandba.And(cm => cm.Title.Contains(title));
            if (!string.IsNullOrEmpty(subTitle)) whereLandba = whereLandba.And(cm => cm.SubTitle.Contains(subTitle));
            if (!string.IsNullOrEmpty(tag)) whereLandba = whereLandba.And(cm => cm.Tags.Contains(tag));
            if (!string.IsNullOrEmpty(keyword)) whereLandba = whereLandba.And(cm => cm.Article.Content.Contains(keyword) || cm.Inputer.UserName.Contains(keyword) || cm.Article.Author.Contains(keyword) || cm.Title.Contains(keyword) || cm.SubTitle.Contains(keyword) || cm.Tags.Contains(keyword));
            if (categoryID > 0) whereLandba = whereLandba.And(com => com.CategoryID == categoryID || com.Category.ParentPath.Contains("|" + categoryID + "|"));
            if (!string.IsNullOrEmpty(inputer)) whereLandba = whereLandba.And(cm => cm.Inputer.UserName == inputer);
            if (fromDate != null) whereLandba = whereLandba.And(cm => cm.ReleaseDate >= fromDate);
            if (toDate != null) whereLandba = whereLandba.And(cm => cm.ReleaseDate <= toDate);
            whereLandba = whereLandba.And(cm => cm.State == state|| (CommonModelState)(-1* (int)cm.State)==state);
            int Index = pageIndex == null ? 1 : (int)pageIndex;
            int Size = pageSize == null ? 1 : (int)pageSize;
            if (pageSize == null) Size = 20;
            //获取实体列表
            var list= FindPageList(Index, Size,out totalRecord, whereLandba, orderType, cm => cm.ReleaseDate);
            totalRecord = pageCount;
            return list;
        }

        public new bool Delete(CommonModel commonModel,bool isSave)
        {
            var nContext = ContextFactory.GetCurrentContext<EFDbContext>() as EFDbContext;
            if (commonModel.Attachment != null) nContext.Attachments.RemoveRange(commonModel.Attachment);
            if (commonModel.Article != null) nContext.Articles.Remove(commonModel.Article);
          //  if (commonModel.Consultation != null) nContext.Consultations.RemoveRange(commonModel.Consultation);
            nContext.CommonModels.Remove(commonModel);
            return isSave ? nContext.SaveChanges() > 0 : true;
        }

        public IQueryable<CommonModel> SearchPageList(out int totalRecord, string keyword, int? pageIndex, int? pageSize)
        {
            var whereLandba = PredicateBuilder.True<CommonModel>();
            if (!string.IsNullOrEmpty(keyword)) whereLandba = whereLandba.And(cm => cm.Article.Content.Contains(keyword) || cm.Inputer.UserName.Contains(keyword) || cm.Article.Author.Contains(keyword) || cm.Title.Contains(keyword) || cm.SubTitle.Contains(keyword) || cm.Tags.Contains(keyword));
            int Index = pageIndex == null ? 1 : (int)pageIndex;
            int Size = pageSize == null ? 1 : (int)pageSize;
            if (pageSize == null) Size = 20;
            var list= base.FindPageList(Index, Size,out totalRecord, whereLandba, OrderType.Desc, cm => cm.ReleaseDate);
            return list;
        }
        /// <summary>
        /// 通过栏目ID查找文章
        /// </summary>
        /// <param name="categoryID">栏目ID</param>
        /// <returns>文章列表</returns>
        public IQueryable<CommonModel> FindByCategory(int number,int categoryID)
        {
            return FindList(0, com => (com.CategoryID == categoryID|| com.Category.ParentPath.Contains("|"+categoryID+"|"))&&com.State==CommonModelState.Normal&&!com.isPage, OrderType.Desc, com => com.ReleaseDate);
        }
    }
}
