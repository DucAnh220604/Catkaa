UPDATE Hotels SET MainImageUrl = 'https://images.unsplash.com/photo-1542314831-c53cd3816002?w=800&q=80' WHERE Id % 7 = 0;
UPDATE Hotels SET MainImageUrl = 'https://images.unsplash.com/photo-1551882547-ff40c0d1398c?w=800&q=80' WHERE Id % 7 = 1;
UPDATE Hotels SET MainImageUrl = 'https://images.unsplash.com/photo-1455587734955-082b2417b08b?w=800&q=80' WHERE Id % 7 = 2;
UPDATE Hotels SET MainImageUrl = 'https://images.unsplash.com/photo-1582719478250-c89fae423c23?w=800&q=80' WHERE Id % 7 = 3;
UPDATE Hotels SET MainImageUrl = 'https://images.unsplash.com/photo-1571003123894-1f053c6c8c0f?w=800&q=80' WHERE Id % 7 = 4;
UPDATE Hotels SET MainImageUrl = 'https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=800&q=80' WHERE Id % 7 = 5;
UPDATE Hotels SET MainImageUrl = 'https://images.unsplash.com/photo-1496417267426-8c5152c4ce8e?w=800&q=80' WHERE Id % 7 = 6;

UPDATE Rooms SET MainImageUrl = 'https://images.unsplash.com/photo-1590490360182-c33d57733427?w=600&q=80' WHERE Id % 6 = 0;
UPDATE Rooms SET MainImageUrl = 'https://images.unsplash.com/photo-1566665797739-1674de7a421a?w=600&q=80' WHERE Id % 6 = 1;
UPDATE Rooms SET MainImageUrl = 'https://images.unsplash.com/photo-1578683010236-d716f9a3f461?w=600&q=80' WHERE Id % 6 = 2;
UPDATE Rooms SET MainImageUrl = 'https://images.unsplash.com/photo-1598928506311-c55d484beb96?w=600&q=80' WHERE Id % 6 = 3;
UPDATE Rooms SET MainImageUrl = 'https://images.unsplash.com/photo-1582719478250-c89fae423c23?w=600&q=80' WHERE Id % 6 = 4;
UPDATE Rooms SET MainImageUrl = 'https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=600&q=80' WHERE Id % 6 = 5;
