const form = document.getElementById("loginForm");

form.addEventListener("submit", login);

async function login(e) {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const pass = document.getElementById("password").value;

    const response = await fetch("https://localhost:7175/api/Authentication/Login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            email: email,
            password: pass
        })
    });

    if (!response.ok) {
        console.log("Login failed");
        return;
    }

    const data = await response.json();
    console.log(data);

    console.log("Email before save:", email);

    localStorage.setItem("UserEmail", email);

    console.log("Email after save:", localStorage.getItem("UserEmail"));

    window.location.href = "VerifiyOpt.html";
}