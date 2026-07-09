INSERT INTO Categories (Name)
VALUES
('Sculptures'),
('Pottery'),
('Architectural Elements'),
('Inscriptions'),
('Jewelry'),
('Coins');

INSERT INTO Museums
(Name, Description, Location, OpeningHours, Phone, Email, Website)
VALUES
(
'Acropolis Museum',
'The Acropolis Museum is one of the most important archaeological museums in Greece. It houses artifacts from the Acropolis of Athens, including sculptures, pottery, inscriptions, and objects from the Archaic, Classical, and Roman periods.',
'15 Dionysiou Areopagitou Street, Athens, Greece',
'09:00 - 17:00',
'+30 210 9000900',
'info@theacropolismuseum.gr',
'https://www.theacropolismuseum.gr'
);

INSERT INTO Exhibits (Title, Description, Year, ImageUrl, CategoryId)
VALUES
('Caryatid', 'One of the six famous Caryatids from the Erechtheion temple.', -420, '/images/caryatid.jpg', 1),
('Kritios Boy', 'Marble statue marking the transition to Classical Greek sculpture.', -480, '/images/kritios-boy.jpg', 1),
('Moschophoros', 'Archaic statue of a man carrying a calf.', -570, '/images/moschophoros.jpg', 1),
('Kore 674', 'Archaic female statue dedicated on the Acropolis.', -510, '/images/kore674.jpg', 1),
('Parthenon Frieze Slab', 'Marble slab from the Parthenon depicting the Panathenaic procession.', -440, '/images/parthenon-frieze.jpg', 3),
('Parthenon Metope', 'Marble metope showing scenes from Greek mythology.', -445, '/images/metope.jpg', 3),
('Attic Black-Figure Amphora', 'Ceramic amphora decorated in the black-figure technique.', -540, '/images/black-amphora.jpg', 2),
('Attic Red-Figure Krater', 'Ceramic mixing vessel decorated in the red-figure technique.', -430, '/images/red-krater.jpg', 2),
('Marble Decree Inscription', 'Public decree carved on marble.', -350, '/images/decree.jpg', 4),
('Funerary Stele', 'Marble funerary monument with relief decoration.', -380, '/images/stele.jpg', 4),
('Golden Olive Wreath', 'Ceremonial gold wreath inspired by olive branches.', -300, '/images/wreath.jpg', 5),
('Athenian Tetradrachm', 'Silver coin depicting Athena and the owl.', -450, '/images/tetradrachm.jpg', 6),
('Peplos Kore', 'Archaic marble statue of a young woman, one of the most famous korai from the Acropolis.', -530, '/images/peplos-kore.jpg', 1),
('Rampin Rider', 'Archaic marble sculpture of a horseman, discovered on the Acropolis.', -550, '/images/rampin-rider.jpg', 1),
('Temple Pediment Sculpture', 'Marble sculpture from the pediment of an ancient temple.', -500, '/images/pediment.jpg', 3),
('Bronze Helmet', 'Ancient Greek bronze helmet dedicated to Athena.', -490, '/images/helmet.jpg', 5),
('Bronze Spearhead', 'Bronze spearhead discovered near the Acropolis.', -470, '/images/spear.jpg', 5),
('Marble Relief of Athena', 'Relief depicting the goddess Athena.', -430, '/images/athena-relief.jpg', 1),
('Silver Drachma', 'Ancient silver drachma minted in Athens.', -450, '/images/drachma.jpg', 6),
('Ceremonial Oil Lamp', 'Decorated ceramic oil lamp from the Classical period.', -350, '/images/oil-lamp.jpg', 2);

INSERT INTO Announcements (Title, Content)
VALUES
('New Exhibition Opening', 'A new temporary exhibition about the Acropolis history is now open.'),
('Educational Programs', 'New educational programs for schools are available this month.'),
('Holiday Opening Hours', 'The museum will operate with special opening hours during public holidays.'),
('Guided Tours', 'Daily guided tours are available in English and Greek.'),
('Family Activities', 'Interactive activities for families take place every Saturday.'),
('Museum Café', 'The museum café is open daily from 09:00 to 18:00.'),
('Accessibility', 'Wheelchair access and accessibility services are available throughout the museum.'),
('Summer Events', 'Special cultural events will be held during the summer season.');

INSERT INTO Users (Username, PasswordHash, Role)
VALUES
('admin', '$2a$11$FFPLoV.GuPKEX0E2JP2nrOgwTfoHeEjIB90Vdp8X5384vdzKdvS1O', 'Admin');

INSERT INTO MediaItems (ExhibitId, FileName, FileType, Url)
VALUES
(1, 'caryatid.jpg', 'Image', '/images/caryatid.jpg'),
(1, 'caryatid.mp3', 'Audio', '/audio/caryatid.mp3'),
(2, 'kritios-boy.jpg', 'Image', '/images/kritios-boy.jpg'),
(3, 'moschophoros.jpg', 'Image', '/images/moschophoros.jpg'),
(4, 'kore674.jpg', 'Image', '/images/kore674.jpg'),
(5, 'parthenon-frieze.jpg', 'Image', '/images/parthenon-frieze.jpg'),
(6, 'metope.jpg', 'Image', '/images/metope.jpg'),
(7, 'black-amphora.jpg', 'Image', '/images/black-amphora.jpg'),
(8, 'red-krater.jpg', 'Image', '/images/red-krater.jpg'),
(9, 'decree.jpg', 'Image', '/images/decree.jpg'),
(10, 'stele.jpg', 'Image', '/images/stele.jpg'),
(11, 'wreath.jpg', 'Image', '/images/wreath.jpg'),
(12, 'tetradrachm.jpg', 'Image', '/images/tetradrachm.jpg'),
(13, 'peplos-kore.jpg', 'Image', '/images/peplos-kore.jpg'),
(14, 'rampin-rider.jpg', 'Image', '/images/rampin-rider.jpg'),
(15, 'pediment.jpg', 'Image', '/images/pediment.jpg'),
(16, 'helmet.jpg', 'Image', '/images/helmet.jpg'),
(17, 'spear.jpg', 'Image', '/images/spear.jpg'),
(18, 'athena-relief.jpg', 'Image', '/images/athena-relief.jpg'),
(19, 'drachma.jpg', 'Image', '/images/drachma.jpg'),
(20, 'oil-lamp.jpg', 'Image', '/images/oil-lamp.jpg'),
(5, 'parthenon-frieze.mp4', 'Video', '/videos/parthenon-frieze.mp4');