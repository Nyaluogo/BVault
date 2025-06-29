// Carousel functionality
let currentSlide = 0;
const track = document.getElementById('carousel-track');
const slides = track ? track.children : [];
function showSlide(idx) {
    if (!track || !slides.length) return;
    currentSlide = (idx + slides.length) % slides.length;
    track.scrollTo({
        left: slides[currentSlide].offsetLeft - track.offsetLeft,
        behavior: 'smooth'
    });
}
function prevSlide() { showSlide(currentSlide - 1); }
function nextSlide() { showSlide(currentSlide + 1); }

// Read more functionality for description
document.getElementById('read-more')?.addEventListener('click', function () {
    const more = document.getElementById('more-text');
    if (more.style.display === 'none') {
        more.style.display = 'inline';
        this.textContent = 'Read less';
    } else {
        more.style.display = 'none';
        this.textContent = 'Read more';
    }
});

// Review "Read more" toggle (expand for demo)
function toggleReview(btn) {
    const reviewBody = btn.closest('.review-card').querySelector('.review-body p');
    if (reviewBody.textContent.length < 100) {
        reviewBody.textContent += " (Full review text goes here for demo.)";
        btn.textContent = "Read less";
    } else {
        reviewBody.textContent = "Amazing game! Had a blast playing with friends. Highly recommend.";
        btn.textContent = "Read more";
    }
}

// Optional: dynamic header background color (demo)
document.addEventListener('DOMContentLoaded', () => {
    // Example: change header background based on game theme
    const header = document.querySelector('.dynamic-bg');
    if (header) {
        // Simulate theme color
        header.style.background = "linear-gradient(120deg, #ffd70022 0%, #101014 100%)";
    }
});


// Profile dropdown toggle
document.addEventListener('DOMContentLoaded', function () {
    const btn = document.getElementById('profileDropdownBtn');
    const menu = document.getElementById('profileDropdownMenu');
    if (btn && menu) {
        btn.addEventListener('click', function (e) {
            e.stopPropagation();
            menu.classList.toggle('show');
            btn.setAttribute('aria-expanded', menu.classList.contains('show'));
        });
        document.addEventListener('click', function () {
            menu.classList.remove('show');
            btn.setAttribute('aria-expanded', 'false');
        });
    }
});

//drag and drop handler
document.addEventListener('DOMContentLoaded', function () {
    const uploadBox = document.getElementById('logo-upload-box');
    if (uploadBox) {
        uploadBox.addEventListener('dragover', function (e) {
            e.preventDefault();
            uploadBox.classList.add('dragover');
        });
        uploadBox.addEventListener('dragleave', function (e) {
            e.preventDefault();
            uploadBox.classList.remove('dragover');
        });
        uploadBox.addEventListener('drop', function (e) {
            e.preventDefault();
            uploadBox.classList.remove('dragover');
            const files = e.dataTransfer.files;
            if (files.length) {
                document.getElementById('LogoFile').files = files;
                uploadBox.textContent = files[0].name;
            }
        });
        uploadBox.addEventListener('click', function () {
            document.getElementById('LogoFile').click();
        });
        document.getElementById('LogoFile').addEventListener('change', function (e) {
            if (e.target.files.length) {
                uploadBox.textContent = e.target.files[0].name;
            }
        });
    }
});

document.addEventListener('DOMContentLoaded', function () {
    var uploadBox = document.getElementById('logo-upload-box');
    var fileInput = document.getElementById('ImageFile');
    if (uploadBox && fileInput) {
        uploadBox.addEventListener('click', function () {
            fileInput.click();
        });
        uploadBox.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' || e.key === ' ') {
                fileInput.click();
                e.preventDefault();
            }
        });
        fileInput.addEventListener('change', function () {
            if (fileInput.files && fileInput.files.length > 0) {
                uploadBox.textContent = fileInput.files[0].name;
            } else {
                uploadBox.textContent = 'Upload Logo';
            }
        });
    }
});