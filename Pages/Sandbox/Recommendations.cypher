match (user:User)-[likes:LIKES]->(paper:Paper)
// where paper.Name contains $term
return user, likes, paper, count(*) as occurrence
order by occurrence desc
// limit coalesce($limit, 50)