using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace fuel_manager_web_api.Models;

[NotMapped]
public class LinkDto(int id, string href, string rel, string metodo)
{
    public int Id { get; set; } = id;
    public string Href { get; set; } = href;
    public string Rel { get; set; } = rel;
    public string Metodo { get; set; } = metodo;
}

public class LinksHATEOS
{
    public List<LinkDto> Links { get; set; } = [];
}