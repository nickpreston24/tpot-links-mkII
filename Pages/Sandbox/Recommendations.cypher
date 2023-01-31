match (user:User)-[likes:LIKES]->(paper:Paper)
where paper.Name contains "Faith"
return user, likes, paper, count(*) as occurrence
order by occurrence desc
limit 50