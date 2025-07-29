<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Code Editor App - Home</title>
    <link rel="stylesheet" href="styles.css">
    <link href='https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css' rel='stylesheet'>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</head>
<body>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">EasyEdit</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="#hero">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link active" href="#features">Features</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link active" href="#preview">Preview</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link active" href="#sample">Samples</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <div class="banner" id="hero">
        <div class="content">
            <h1>Code Smarter, Build Faster</h1>
            <p>Experience a powerful, interactive code editor for all your development needs.</p>
            <div class="buttons">
                <a href="signUp.aspx" class="btn get-started">Get Started</a>
                <a href="#features" class="btn learn-more">Learn More</a>
            </div>
        </div>
    </div>

    <!--Features Section-->
    <section class="features" id="features">
        <div class="container">
            <h2>Features</h2>
            <div class="feature-list">
                <div class="feature-card">
                    <img src="rb_8642.png" alt="Collaboration Icon">
                    <h3>Real-time Collaboration</h3>
                    <p>Work with teammates in real-time, no matter where you are.</p>
                </div>
                <div class="feature-card">
                    <img src="rb_8193.png" alt="Syntax Highlighting Icon">
                    <h3>Syntax Highlighting</h3>
                    <p>Color-coded syntax for better readability.</p>
                </div>
                <div class="feature-card">
                    <img src="rb_2148244398.png" alt="Multi-language Support Icon">
                    <h3>Multi-Language Support</h3>
                    <p>Supports a variety of programming languages.</p>
                </div>
                <div class="feature-card">
                    <img src="computer-program-code.jpg" alt="Customizable Themes Icon">
                    <h3>Customizable Themes</h3>
                    <p>Choose themes that suit your style.</p>
                </div>
            </div>
        </div>
    </section>
    <!-- Preview Section -->
    <section id="preview" class="preview-section">
        <div class="editor-preview">
            <div id="preview-text">
                <h2>
                    Code With Us!<br /> Anytime.
                </h2>
                <pre>Use editor to create, debug, 
and build your projects.</pre>
                <button class="cta-btn btn-danger" id="sign-up-button">Sign In</button>
            </div>
            <div id="carouselExampleControls" class="carousel slide" data-bs-ride="carousel">
                <div class="carousel-inner">
                    <div class="carousel-item active">
                        <img src="Editor_Image_Preview.png" class="d-block w-100" alt="Editor Preview 1">
                    </div>
                    <div class="carousel-item">
                        <img src="Screenshot 2024-11-11 210249.png" class="d-block w-100" alt="Editor Preview 2">
                    </div>
                </div>
                <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleControls" data-bs-slide="prev">
                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span class="visually-hidden">Previous</span>
                </button>
                <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleControls" data-bs-slide="next">
                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                    <span class="visually-hidden">Next</span>
                </button>
            </div>
        </div>
    </section>


    <section>
        <div class="practice" id="sample">
            <!-- Image Container -->
            <div id="image" class="image">
                <img src="compiler.png" title="Python-Program" alt="not_found" />
            </div>

            <!-- Cards Container -->
            <div class="my_cards">
                <h2>Practice with our Online Compilers!</h2>
                <p>
                    We believe coding should be accessible to all. So we made our own compilers<br />
                    for web and mobile. And it's free!
                </p>
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Python Programming</h5>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">JavaScript Programming</h5>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">C Programming</h5>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">C++ Programming</h5>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section>
        <footer class="footer">
            <div class="footer-section about">
                <h2>About Us</h2>
                <p>We are a company dedicated to providing the best services to our clients with quality and integrity.</p>
            </div>
            <div class="footer-section links">
                <h2>Quick Links</h2>
                <ul>
                    <li><a href="#hero">Home</a></li>
                    <li><a href="#features">Services</a></li>
                    <li><a href="#preview">Preview</a></li>
                    <li><a href="#sample">Samples</a></li>
                </ul>
            </div>
            <div class="footer-section contact">
                <h2>Contact Us</h2>
                <p>Email: info@company.com</p>
                <p>Phone: +91 620 69X XXXX</p>
                <p>Address: 1234 Street Name, City, Country</p>
            </div>
        </footer>
    </section>
    <script>
        document.getElementById('sign-up-button').addEventListener('click', () => {
            window.location.href = "LoginForm.aspx";
        })
    </script>
</body>
</html>