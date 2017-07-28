using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArlecClock.DAL
{
    public class WMSDBEntitiesRepository : IDisposable
    {

        private WMSDBEntities context = new WMSDBEntities();
        /*
                public IEnumerable<Department> GetDepartments()
                {
                    return context.Departments.Include("Person").ToList();
                }

                    private bool disposedValue = false;

                    protected virtual void Dispose(bool disposing)
                    {
                        if (!this.disposedValue)
                        {
                            if (disposing)
                            {
                                context.Dispose();
                            }
                        }
                        this.disposedValue = true;
                    }*/

        public void Dispose()
        {
            // Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
