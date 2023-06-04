MATCH (page:Page)
// OPTIONAL MATCH (a)-[r:LINKS_TO]-(x)
WHERE 
page.Content =~ $regex
  OR page.Title =~ $regex
  OR page.Slug =~ $regex
  OR page.Excerpt =~ $regex
  OR page.Id =~ $regex
  OR page.Categories =~ $regex
RETURN *


// MATCH (page:Page)-[r]->(m)
// WHERE 'Page' IN labels(page) 
//  AND
//   (page.Title =~ $regex OR toLower(page.Title) CONTAINS toLower($term)
//     OR page.Slug =~ $regex OR toLower(page.Slug) CONTAINS toLower($term)
//     OR page.Excerpt =~ $regex OR toLower(page.Excerpt) CONTAINS toLower($term)
//   )
//   AND ($category = '' OR toLower(page.Categories) contains toLower($category))
// RETURN page, r, m


// //        AND (:EMP_ID IS NULL OR "EMPLOYEE_ID"=:EMP_ID) 
