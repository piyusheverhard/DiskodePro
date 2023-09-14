CREATE
DATABASE diskode;

USE
diskode;

CREATE TABLE users
(
    user_id  INT AUTO_INCREMENT PRIMARY KEY,
    name     VARCHAR(255)        NOT NULL,
    email    VARCHAR(255) UNIQUE NOT NULL,
    password VARCHAR(255)        NOT NULL
);

CREATE TABLE posts
(
    post_id    INT AUTO_INCREMENT PRIMARY KEY,
    creator    INT       NOT NULL,
    content    TEXT      NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (creator) REFERENCES users (user_id) ON DELETE CASCADE
);

CREATE TABLE comments
(
    comment_id        INT AUTO_INCREMENT PRIMARY KEY,
    post_id           INT       NOT NULL,
    creator           INT       NOT NULL,
    content           TEXT      NOT NULL,
    created_at        TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    is_deleted        BOOLEAN   NOT NULL DEFAULT FALSE,
    parent_comment_id INT                DEFAULT NULL,
    FOREIGN KEY (post_id) REFERENCES posts (post_id) ON DELETE CASCADE,
    FOREIGN KEY (creator) REFERENCES users (user_id),
    FOREIGN KEY (parent_comment_id) REFERENCES comments (comment_id)
);

CREATE TABLE tags
(
    tag_id   INT AUTO_INCREMENT PRIMARY KEY,
    tag_name VARCHAR(255) NOT NULL
);

CREATE TABLE tag_post_mapping
(
    tag_id  INT NOT NULL,
    post_id INT NOT NULL,
    PRIMARY KEY (tag_id, post_id),
    FOREIGN KEY (tag_id) REFERENCES tags (tag_id),
    FOREIGN KEY (post_id) REFERENCES posts (post_id) ON DELETE CASCADE
);

CREATE TABLE post_like_mapping
(
    post_id INT NOT NULL,
    user_id INT NOT NULL,
    PRIMARY KEY (post_id, user_id),
    FOREIGN KEY (post_id) REFERENCES posts (post_id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE CASCADE
);

CREATE TABLE comment_like_mapping
(
    comment_id INT NOT NULL,
    user_id    INT NOT NULL,
    PRIMARY KEY (comment_id, user_id),
    FOREIGN KEY (comment_id) REFERENCES comments (comment_id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE CASCADE
);

CREATE TABLE followers
(
    follower_id  INT NOT NULL,
    following_id INT NOT NULL,
    PRIMARY KEY (follower_id, following_id),
    FOREIGN KEY (follower_id) REFERENCES users (user_id) ON DELETE CASCADE,
    FOREIGN KEY (following_id) REFERENCES users (user_id) ON DELETE CASCADE
);

CREATE TABLE favorites
(
    user_id INT NOT NULL,
    post_id INT NOT NULL,
    PRIMARY KEY (user_id, post_id),
    FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE CASCADE,
    FOREIGN KEY (post_id) REFERENCES posts (post_id) ON DELETE CASCADE
);


-- Entries for the 'users' table
INSERT INTO users (name, email, password)
VALUES ('John Doe', 'johndoe@example.com', 'password1'),
       ('Jane Doe', 'janedoe@example.com', 'password2'),
       ('Bob Smith', 'bobsmith@example.com', 'password3');

-- Entries for the 'posts' table
INSERT INTO posts (creator, content)
VALUES (1, 'This is my first post.'),
       (2, 'I had a great day today!'),
       (3, 'Just finished reading a fantastic book.');

-- Entries for the 'comments' table
INSERT INTO comments (post_id, creator, content, parent_comment_id)
VALUES (1, 2, 'Great post!', NULL),
       (1, 3, 'I agree, it was a good read.', NULL),
       (2, 1, 'Glad to hear it!', NULL),
       (3, 2, 'What book was it?', NULL),
       (3, 1, 'It was "To Kill a Mockingbird".', NULL),
       (3, 2, 'Oh, I love that book!', 5),
       (3, 1, 'Me too!', 6);

-- Entries for the 'tags' table
INSERT INTO tags (tag_name)
VALUES ('travel'),
       ('food'),
       ('books'),
       ('movies');

-- Entries for the 'tag_post_mapping' table
INSERT INTO tag_post_mapping (tag_id, post_id)
VALUES (1, 1),
       (1, 2),
       (2, 2),
       (3, 3),
       (4, 3);

-- Entries for the 'post_like_mapping' table
INSERT INTO post_like_mapping (post_id, user_id)
VALUES (1, 2),
       (2, 1),
       (2, 3),
       (3, 1),
       (3, 2);

-- Entries for the 'comment_like_mapping' table
INSERT INTO comment_like_mapping (comment_id, user_id)
VALUES (1, 1),
       (1, 2),
       (2, 2),
       (3, 1),
       (4, 3),
       (5, 2),
       (6, 1),
       (7, 3);

-- Entries for the 'followers' table
INSERT INTO followers (follower_id, following_id)
VALUES (1, 2),
       (1, 3),
       (2, 1),
       (3, 1);

-- Entries for the 'favorites' table
INSERT INTO favorites (user_id, post_id)
VALUES (1, 1),
       (1, 3),
       (2, 2),
       (2, 3),
       (3, 1);
