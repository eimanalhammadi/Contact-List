using System;
using System.Collections.Generic;

namespace ContactsList.Models;

public partial class Contact
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string City { get; set; } = null!;
}
