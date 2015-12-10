using System.Collections.Generic;
using System.Linq;
using You.Models;
using You.Data.Types;

namespace You.Service
{
    public class CategoryService : BaseService<Category>
    {
        private List<Category> Categories;

        public Category Add(Category cat)
        {
            if (cat.ParentId != 0)
            {
                Category parent = Find(cat.ParentId);
                cat.ParentPath = parent.ParentPath + parent.CategoryID+'|';
            }
            else cat.ParentPath="|0|";
            return base.Add(cat);
        }
     
        public bool Delete(int id)
        {
            Category _category = Find(id);
            if (_category == null) return false;
            IQueryable<Category> list = FindList(0, c => c.ParentId == id, OrderType.No, c => c.CategoryID);
            foreach (var item in list)
            {
                item.ParentId = 0;
                Update(item, false);
            }
            Delete(_category,false);
            if (Save() > 0) return true;
            return false;
        }

        public bool Update(Category cat)
        {
            Category _category = Find(cat.CategoryID);
            if (_category.ParentId != cat.ParentId)
            {
                var list = FindList(0, c => c.ParentPath.Contains(_category.ParentPath), OrderType.No, c => c.CategoryID);
                string parentPath = _category.ParentPath;

                if (cat.ParentId != 0)
                {
                    var _parent = Find(cat.ParentId);
                    cat.ParentPath = _parent.ParentPath + _parent.CategoryID + '|';
                }
                else cat.ParentPath = "|0|";
                
                foreach (var item in list)
                {
                    item.ParentPath = cat.ParentPath + item.ParentPath.Replace(parentPath, "");
                    Update(item, false);
                }
            }
            Update(cat, false);
            if (Save() > 0) return true;
            else return false;
        }

        public Category Find(string name) { return Find(c => c.Name == name); }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="cat">分类实体</param>
        /// <param name="parentId">父级分类Id</param>
        /// <returns></returns>
        //public Category Add(Category cat) 
        //{
        //    if (cat.ParentId >0)
        //    {
        //        Category _cat = Find(cat.ParentId);
        //        _cat.isLeft = false;
        //        Update(_cat);
        //        return Add(cat);
        //    }
        //    else
        //    {
        //        return Add(cat);
        //    }
        //}

        /// <summary>
        /// 获取子分类
        /// </summary>
        /// <param name="parentId">父级分类Id</param>
        /// <returns>子分类列表</returns>
        public List<Tree> FindChildren(int parentId)
        {
            try
            {
                return Categories.Where(cat => cat.ParentId == parentId && cat.State != ItemState.Deleted).OrderBy(cat=>cat.Order).Select(cat => new Tree { id = cat.CategoryID, text = cat.Name }).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<Category> FindList( CategoryType? type)
        {
            return base.FindList(0, cat => cat.Type == type , OrderType.No, cat => cat.CategoryID).ToList();
        }
        /// <summary>
        /// 获取多级分类列表
        /// </summary>
        /// <returns>分类列表</returns>
        public List<Tree> FindTree( CategoryType? type)
        {
            Categories = FindList(type);
            List<Tree> categorylist = FindChildren(0);
            if (categorylist != null && categorylist.Count > 0) getChildenIntoList(ref categorylist);
            //return new Tree { id = 0, text = "根分类", nodes = categorylist };
            return categorylist;
        }

        /// <summary>
        /// 递归获取分类列表
        /// </summary>
        /// <param name="parents">父级分类列表</param>
        private void getChildenIntoList(ref List<Tree> parents)
        {
            foreach (var cat in parents)
            {
                List<Tree> children = FindChildren(cat.id);
                cat.nodes = children;
                if (children != null && children.Count > 0) getChildenIntoList(ref children);
                else cat.nodes = null;
            }
        }

        public class Tree
        {
            public int id { get; set; }
            public string text { get; set; }
            public List<Tree> nodes { get; set; }
        }
    }
}
