MATCH (page:Page)-[r]->(m)
WHERE 'Page' IN labels(page) 
 AND
  (page.Title =~ $regex
    OR page.Slug =~ $regex
    OR page.Excerpt =~ $regex
  )
RETURN page, r, m