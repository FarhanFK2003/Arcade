

const validateEmailStructure = (email) => {
    return String(email)
        .toLowerCase()
        .match(
            /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        );
};

const emailInputs = document.getElementsByClassName('emailInput');
const submitButton = document.querySelector('button[type="submit"]');

function updateButtonState() {
    let allValid = true;
    Array.from(emailInputs).forEach(emailInput => {
        const isValid = validateEmailStructure(emailInput.value);
        if (!isValid) { 
            allValid = false;
        }
    });
    submitButton.disabled = !allValid;
}

submitButton.disabled = true;

Array.from(emailInputs).forEach(emailInput => {
    emailInput.addEventListener('input', updateButtonState);
});
