using Azure;
using CodeGenerate.CustomTagHelpers;
using CodeGenerate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace CodeGenerate.CustomTagHelpers
{
    [HtmlTargetElement("td", Attributes = "i-role")]
    public class RoleUsersTH : TagHelper
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public RoleUsersTH(UserManager<User> usermgr, RoleManager<IdentityRole> rolemgr)
        {
            userManager = usermgr;
            roleManager = rolemgr;
        }

        [HtmlAttributeName("i-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in userManager.Users)
                {
                    if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                        names.Add(user.UserName);
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));

            //output.Content.SetContent("hung");
        }
    }
}

/*
< td i - role = "@role.Id" ></ td >
    This attribute will call a Custom Tag Helper which will modify this td element 
    to display a list of all the Users who are members of each Role.

    This Custom Tag Helper operates on the td elements having an i-role attribute. 
    This attribute is used to receive the id of the role that is being processed.

    We need to update the View Imports file (_ViewImports.cshtml) to import:

1.Microsoft.AspNetCore.Identity namespace on the views.
2. The custom tag helper called RoleUsersTH.cs

    @using Microsoft.AspNetCore.Identity
    @addTagHelper Identity.CustomTagHelpers.*, CodeGenerate
*/