MATCH (page:Page)-[r]->(m)
WHERE 'Page' IN labels(page) 
 AND
  (page.Title =~ $regex OR toLower(page.Title) CONTAINS toLower($term)
    OR page.Slug =~ $regex OR toLower(page.Slug) CONTAINS toLower($term)
    OR page.Excerpt =~ $regex OR toLower(page.Excerpt) CONTAINS toLower($term)
  )
  AND ($category = '' OR toLower(page.Categories) contains toLower($category))
RETURN page, r, m


//        AND (:EMP_ID IS NULL OR "EMPLOYEE_ID"=:EMP_ID) 
