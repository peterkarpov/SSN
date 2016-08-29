using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Concrete
{
    public partial class EFRepository : IRepository
    {
        public IEnumerable<Photobook> Photobooks
        {
            get
            {
                return Context.Photobooks;
            }
        }

        public bool SavePhotobook(Photobook photobook)
        {
            if (photobook.PhotobookId == default(Guid))
            {
                photobook.PhotobookId = Guid.NewGuid();
                Context.Photobooks.Add(photobook);
            }
            else
            {
                Photobook dbEntry = Context.Photobooks.Find(photobook.PhotobookId);
                if (dbEntry != null)
                {
                    dbEntry.Title = photobook.Title;
                    dbEntry.PhotobookId = photobook.PhotobookId;

                }
            }

            return Context.SaveChanges() > 0;
        }


    }
}