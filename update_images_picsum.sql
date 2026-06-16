UPDATE Hotels SET MainImageUrl = 'https://picsum.photos/seed/' + CAST(Id AS VARCHAR) + '/800/500';
UPDATE Rooms SET MainImageUrl = 'https://picsum.photos/seed/room' + CAST(Id AS VARCHAR) + '/800/500';
