CREATE TABLE IF NOT EXISTS OSD(
	id INTEGER PRIMARY KEY,
	path text NOT NULL,
	version text,
	currentURLOrFilePathForAssistantButton text,
	uaflMode text
);