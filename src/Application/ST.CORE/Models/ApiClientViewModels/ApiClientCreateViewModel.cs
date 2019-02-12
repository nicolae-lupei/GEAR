using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ST.CORE.Models.ApiClientViewModels
{
	public class ApiClientCreateViewModel
	{
		[Display(Name = "Hybrid", Description = "Hybrid Authentication flow", Prompt = "Hybrid Authentication flow")]
		public bool HybridGrantType { get; set; }

		[Display(Name = "ClientCredentials", Prompt = "Client Credentials authentication flow")]
		public bool ClientCredentialsGrantType { get; set; }

		[Display(Name = "Implicit garend type")]
		public bool ImplicitGrantType { get; set; }

		[Display(Name = "Resource owner password grand type")]
		public bool ResourceOwnerPasswordGrantType { get; set; }

		[Display(Name = "Authorization code grand type")]
		public bool AuthorizationCodeGrantType { get; set; }

		[Required]
		[Display(Name = "Client Name")]
		public string ClientName { get; set; }

		[Required]
		[Display(Name = "Client Id")]
		public string ClientId { get; set; }

		public IEnumerable<string> ChosenApiScopes { get; set; }

		[Required]
		[Display(Name = "Client URI")]
		[StringLength(50)]
		public string ClientUri { get; set; }
	}


	public class ApiClientGeneralViewModel
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "Client Name")]
		public string ClientName { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required]
		[Display(Name = "Client Uri")]
		public string ClientUri { get; set; }
	}

	public class ApiClientRoles
	{
	}

	public class ApiClientPermissions
	{
		public Guid PermissionId { get; set; }
		[Display(Name = "Perrmisiion Name")]
		public string PermissionName { get; set; }
		[Display(Name = "Client Id")]
		public int ClientId { get; set; }
	}
}