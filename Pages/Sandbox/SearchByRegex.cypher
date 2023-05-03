MATCH (page)-[rel]->(m)
WHERE "Page" IN LABELS(page) 
AND page.Title =~ '(?i)^$Term.*'
// AND page.Slug =~ '(?i)^P.*'
// OR page.Excerpt =~ '(?i)^P.*'
RETURN page,rel