using System.Linq;
using You.Models;
using You.Data.Types;
using You.Core;

namespace You.Service
{
    /// <summary>
    /// 附件服务
    /// <remarks>
    /// 创建：2014.03.05
    /// </remarks>
    /// </summary>
    public class AttachmentService : BaseService<Attachment>
    {
        public IQueryable<Attachment> FindList(CommonModel commonModel, string inputer, AttachmentType? type)
        {
            var whereLandba = PredicateBuilder.True<Attachment>();
            if (string.IsNullOrEmpty(inputer)) whereLandba = whereLandba.And(a => a.Inputer == inputer);
            if (type != null) whereLandba = whereLandba.And(a => a.Type == type);
            var _attachemts = FindList(0, whereLandba, OrderType.No, a => a.AttachmentID);
            return _attachemts;
        }

        public IQueryable<Attachment> FindList(CommonModel commonModel,  string inputer, AttachmentType? type, bool withModelNull)
        {
            var whereLandba = PredicateBuilder.True<Attachment>();
            if (withModelNull) whereLandba = whereLandba.And(a => a.CommonModels.Contains(commonModel) || a.CommonModels.Count == 0);
            else whereLandba = whereLandba.And(a => a.CommonModels.Contains(commonModel));
            if (string.IsNullOrEmpty(inputer)) whereLandba = whereLandba.And(a => a.Inputer == inputer);
            if (type != null) whereLandba = whereLandba.And(a => a.Type == type);
            var _attachemts = FindList(0, whereLandba, OrderType.No, a => a.AttachmentID);
            return _attachemts;
        }
    }
}
