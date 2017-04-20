using System;
using System.Security.Principal;
using System.Web;
using System.Linq;
using AutoMapper;

namespace KMHC.Infrastructure.Security
{
    public class ICustomPrincipal : LTCUserData, IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public string[] roles { get; set; }

        public ICustomPrincipal(LTCUserData clientUserData)
        {
            this.Identity = new GenericIdentity(string.Format("{0}_{1}", clientUserData.UserId, clientUserData.EmpName));
            Mapper.CreateMap<LTCUserData, ICustomPrincipal>();
            Mapper.Map(clientUserData, this);
        }

        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
