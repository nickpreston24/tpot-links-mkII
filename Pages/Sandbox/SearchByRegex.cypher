MATCH (page:Page)-[r]->(m)
WHERE 'Page' IN labels(page) 
 AND
  (page.Title =~ $regex OR toLower(page.Title) CONTAINS toLower($term)
    OR page.Slug =~ $regex OR toLower(page.Slug) CONTAINS toLower($term)
    OR page.Excerpt =~ $regex OR toLower(page.Excerpt) CONTAINS toLower($term)
  )
RETURN page, r, m