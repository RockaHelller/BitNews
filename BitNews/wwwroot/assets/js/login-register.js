document.addEventListener('DOMContentLoaded', function () {
    const passwordInput1 = document.getElementById('id_password1');
    const togglePassword1 = document.getElementById('togglePassword1');
    const togglePassword2 = document.getElementById('togglePassword2');

    const passwordInput2 = document.getElementById('id_password2');
    const togglePassword3 = document.getElementById('togglePassword3');
    const togglePassword4 = document.getElementById('togglePassword4');

    function togglePasswordVisibility(inputElement, icon1, icon2) {
        if (inputElement.type === 'password') {
            inputElement.type = 'text';
            icon1.classList.add('d-none');
            icon2.classList.remove('d-none');
        } else {
            inputElement.type = 'password';
            icon2.classList.add('d-none');
            icon1.classList.remove('d-none');
        }
    }

    if (passwordInput1 && togglePassword1 && togglePassword2) {
        togglePassword1.addEventListener('click', function () {
            togglePasswordVisibility(passwordInput1, togglePassword1, togglePassword2);
        });

        togglePassword2.addEventListener('click', function () {
            togglePasswordVisibility(passwordInput1, togglePassword1, togglePassword2);
        });
    }

    if (passwordInput2 && togglePassword3 && togglePassword4) {
        togglePassword3.addEventListener('click', function () {
            togglePasswordVisibility(passwordInput2, togglePassword3, togglePassword4);
        });

        togglePassword4.addEventListener('click', function () {
            togglePasswordVisibility(passwordInput2, togglePassword3, togglePassword4);
        });
    }
});
