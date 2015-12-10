using System.Collections.Generic;
using System.Linq;
using You.Data.Types;
using You.Models;

namespace You.Service
{
    public class TagService:BaseService<Tag>
    {
        /// <summary>
        /// 添加文章时对标签的处理
        /// </summary>
        /// <param name="article">文章实体</param>
        /// <returns>更新行数</returns>
        public int Update(CommonModel article, string add="", string remove="")
        {
            List<Tag> _tags = FindAll().ToList();
            if (!string.IsNullOrEmpty(add))
            {
                string[] addTags = add.Split(',');
                foreach (var tag in addTags)
                {
                    var _t = _tags.SingleOrDefault(t => t.Name == tag);
                    if (_t == null)
                    {
                        var t = new Tag() { Name = tag, Articles = new List<CommonModel>() };
                        t.Articles.Add(article);
                        Add(t, false);
                    }
                    else if ( _t.State != ItemState.Deleted)
                    {
                        _t.Articles.Add(article);
                        Update(_t, false);
                    }
                }
            }
            if (!string.IsNullOrEmpty(remove))
            {
                string[] removeTags = remove.Split(',');
                foreach (var tag in _tags)
                {
                    if (removeTags.SingleOrDefault(rt => rt == tag.Name) != null)
                    {
                        tag.Articles.Remove(article);
                        Update(tag, false);
                    }
                }
            }
            return Save();
        }
        
        public List<Tag> Find(string tag) { return FindList(0,u => u.Name.Contains(tag),OrderType.Asc,u=>u.Hit).ToList(); }
    }
}
