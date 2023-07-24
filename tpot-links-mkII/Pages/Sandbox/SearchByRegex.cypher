MATCH (paper:Paper)
// OPTIONAL MATCH (a)-[r:LINKS_TO]-(x)
WHERE 
paper.Content =~ $regex
  OR paper.Title =~ $regex
  OR paper.Excerpt =~ $regex
  OR paper.Slug =~ $regex
  // OR paper.Id =~ $regex
  // OR paper.Categories =~ $regex
RETURN *
LIMIT $limit


// MATCH (paper:Paper)-[r]->(m)
// WHERE 'Paper' IN labels(paper) 
//  AND
//   (paper.Title =~ $regex OR toLower(paper.Title) CONTAINS toLower($term)
//     OR paper.Slug =~ $regex OR toLower(paper.Slug) CONTAINS toLower($term)
//     OR paper.Excerpt =~ $regex OR toLower(paper.Excerpt) CONTAINS toLower($term)
//   )
//   AND ($category = '' OR toLower(paper.Categories) contains toLower($category))
// RETURN paper, r, m


// //        AND (:EMP_ID IS NULL OR "EMPLOYEE_ID"=:EMP_ID) 
