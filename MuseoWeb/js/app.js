const API_BASE_URL = "https://localhost:7199";


let currentCategoryId = null;
let currentCategoryName = null;
let currentEditingCategory = null;
let currentEditingExhibit = null;


// =====================================================
// ELEMENTS
// =====================================================

const loginForm =
    document.getElementById("loginForm");

const loginSection =
    document.getElementById("loginSection");

const message =
    document.getElementById("message");


const categoriesSection =
    document.getElementById("categoriesSection");

const categoriesContainer =
    document.getElementById("categoriesContainer");

const addCategoryButton =
    document.getElementById("addCategoryButton");


const createCategorySection =
    document.getElementById("createCategorySection");

const createCategoryForm =
    document.getElementById("createCategoryForm");

const createCategoryMessage =
    document.getElementById("createCategoryMessage");

const backFromCategoryCreateButton =
    document.getElementById("backFromCategoryCreateButton");


const editCategorySection =
    document.getElementById("editCategorySection");

const editCategoryForm =
    document.getElementById("editCategoryForm");

const editCategoryMessage =
    document.getElementById("editCategoryMessage");

const deleteCategoryButton =
    document.getElementById("deleteCategoryButton");

const backFromCategoryEditButton =
    document.getElementById("backFromCategoryEditButton");


const exhibitsSection =
    document.getElementById("exhibitsSection");

const exhibitsContainer =
    document.getElementById("exhibitsContainer");

const selectedCategoryTitle =
    document.getElementById("selectedCategoryTitle");

const addExhibitButton =
    document.getElementById("addExhibitButton");

const backToCategoriesButton =
    document.getElementById("backToCategoriesButton");


const createExhibitSection =
    document.getElementById("createExhibitSection");

const createExhibitForm =
    document.getElementById("createExhibitForm");

const createExhibitMessage =
    document.getElementById("createExhibitMessage");

const backFromCreateButton =
    document.getElementById("backFromCreateButton");


const editExhibitSection =
    document.getElementById("editExhibitSection");

const editExhibitForm =
    document.getElementById("editExhibitForm");

const editExhibitMessage =
    document.getElementById("editExhibitMessage");

const deleteExhibitButton =
    document.getElementById("deleteExhibitButton");

const backFromEditButton =
    document.getElementById("backFromEditButton");


const exhibitDetailsSection =
    document.getElementById("exhibitDetailsSection");

const exhibitDetailsContainer =
    document.getElementById("exhibitDetailsContainer");

const mediaContainer =
    document.getElementById("mediaContainer");

const backToExhibitsButton =
    document.getElementById("backToExhibitsButton");


// =====================================================
// LOGIN
// =====================================================

loginForm.addEventListener("submit", async function (event) {

    event.preventDefault();

    const username =
        document.getElementById("username").value;

    const password =
        document.getElementById("password").value;


    const response = await fetch(
        `${API_BASE_URL}/api/auth/login`,
        {
            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                username,
                password
            })
        }
    );


    if (!response.ok) {

        message.textContent =
            "Invalid username or password.";

        return;
    }


    const data =
        await response.json();


    localStorage.setItem(
        "token",
        data.token
    );


    loginSection.classList.add("hidden");

    categoriesSection.classList.remove("hidden");


    await loadCategories();
});


// =====================================================
// CATEGORY HELPERS
// =====================================================

async function uploadCategoryImage(categoryId, file) {

    if (!file) {
        return;
    }


    const token =
        localStorage.getItem("token");


    const formData =
        new FormData();


    formData.append(
        "file",
        file
    );


    const response =
        await fetch(
            `${API_BASE_URL}/api/categories/${categoryId}/image`,
            {
                method: "POST",

                headers: {
                    "Authorization":
                        `Bearer ${token}`
                },

                body:
                    formData
            }
        );


    if (!response.ok) {

        const data =
            await response.json();

        throw new Error(
            data.message ||
            "Could not upload category image."
        );
    }
}


// =====================================================
// LOAD CATEGORIES
// =====================================================

async function loadCategories() {

    const response =
        await fetch(
            `${API_BASE_URL}/api/categories`
        );


    const categories =
        await response.json();


    categoriesContainer.innerHTML =
        "";


    categories.forEach(category => {

        const card =
            document.createElement("div");

        card.className =
            "category-card";


        if (category.imageUrl) {

            const image =
                document.createElement("img");

            image.className =
                "category-image";

            image.src =
                category.imageUrl;

            card.appendChild(image);
        }
        else {

            const placeholder =
                document.createElement("div");

            placeholder.className =
                "category-placeholder";

            placeholder.textContent =
                "No image";

            card.appendChild(placeholder);
        }


        const content =
            document.createElement("div");

        content.className =
            "category-content";


        const name =
            document.createElement("div");

        name.className =
            "category-name";

        name.textContent =
            category.name;


        const editButton =
            document.createElement("button");

        editButton.className =
            "category-edit-button";

        editButton.textContent =
            "Edit";


        editButton.addEventListener(
            "click",
            function (event) {

                event.stopPropagation();

                openEditCategory(category);
            }
        );


        content.appendChild(name);
        content.appendChild(editButton);

        card.appendChild(content);


        card.addEventListener(
            "click",
            async function () {

                currentCategoryId =
                    category.id;

                currentCategoryName =
                    category.name;


                await loadExhibitsByCategory(
                    category.id,
                    category.name
                );
            }
        );


        categoriesContainer.appendChild(card);
    });
}


// =====================================================
// CREATE CATEGORY
// =====================================================

addCategoryButton.addEventListener(
    "click",
    function () {

        categoriesSection.classList.add("hidden");

        createCategorySection.classList.remove("hidden");

        createCategoryForm.reset();
    }
);


createCategoryForm.addEventListener(
    "submit",
    async function (event) {

        event.preventDefault();


        const token =
            localStorage.getItem("token");


        const name =
            document.getElementById(
                "createCategoryName"
            ).value.trim();


        const image =
            document.getElementById(
                "createCategoryImage"
            ).files[0];


        try {

            const response =
                await fetch(
                    `${API_BASE_URL}/api/categories`,
                    {
                        method: "POST",

                        headers: {
                            "Content-Type":
                                "application/json",

                            "Authorization":
                                `Bearer ${token}`
                        },

                        body: JSON.stringify({
                            name: name,
                            imagePath: null
                        })
                    }
                );


            if (!response.ok) {

                const data =
                    await response.json();

                throw new Error(
                    data.message ||
                    "Could not create category."
                );
            }


            const created =
                await response.json();


            if (image) {

                await uploadCategoryImage(
                    created.id,
                    image
                );
            }


            createCategorySection.classList.add(
                "hidden"
            );

            categoriesSection.classList.remove(
                "hidden"
            );


            await loadCategories();
        }
        catch (error) {

            createCategoryMessage.textContent =
                error.message;
        }
    }
);


// =====================================================
// EDIT CATEGORY
// =====================================================

function openEditCategory(category) {

    currentEditingCategory =
        category;


    categoriesSection.classList.add("hidden");

    editCategorySection.classList.remove("hidden");


    document.getElementById(
        "editCategoryName"
    ).value =
        category.name;


    document.getElementById(
        "editCategoryImage"
    ).value =
        "";
}


editCategoryForm.addEventListener(
    "submit",
    async function (event) {

        event.preventDefault();


        const token =
            localStorage.getItem("token");


        const newImage =
            document.getElementById(
                "editCategoryImage"
            ).files[0];


        const name =
            document.getElementById(
                "editCategoryName"
            ).value.trim();


        try {

            const response =
                await fetch(
                    `${API_BASE_URL}/api/categories/${currentEditingCategory.id}`,
                    {
                        method: "PUT",

                        headers: {

                            "Content-Type":
                                "application/json",

                            "Authorization":
                                `Bearer ${token}`
                        },

                        body: JSON.stringify({
                            name: name,

                            imagePath:
                                currentEditingCategory.imagePath
                        })
                    }
                );


            if (!response.ok) {

                throw new Error(
                    "Could not update category."
                );
            }


            if (newImage) {

                await uploadCategoryImage(
                    currentEditingCategory.id,
                    newImage
                );
            }


            currentEditingCategory =
                null;


            editCategorySection.classList.add(
                "hidden"
            );

            categoriesSection.classList.remove(
                "hidden"
            );


            await loadCategories();
        }
        catch (error) {

            editCategoryMessage.textContent =
                error.message;
        }
    }
);


// =====================================================
// DELETE CATEGORY
// =====================================================

deleteCategoryButton.addEventListener(
    "click",
    async function () {

        if (!currentEditingCategory) {
            return;
        }


        if (!confirm(
            `Delete "${currentEditingCategory.name}"?`
        )) {
            return;
        }


        const token =
            localStorage.getItem("token");


        let response =
            await fetch(
                `${API_BASE_URL}/api/categories/${currentEditingCategory.id}`,
                {
                    method: "DELETE",

                    headers: {
                        "Authorization":
                            `Bearer ${token}`
                    }
                }
            );


        if (response.status === 409) {

            const data =
                await response.json();


            if (!confirm(
                `${data.warning}\n\nContinue?`
            )) {
                return;
            }


            response =
                await fetch(
                    `${API_BASE_URL}/api/categories/${currentEditingCategory.id}?force=true`,
                    {
                        method: "DELETE",

                        headers: {
                            "Authorization":
                                `Bearer ${token}`
                        }
                    }
                );
        }


        if (!response.ok) {

            alert(
                "Could not delete category."
            );

            return;
        }


        currentEditingCategory =
            null;


        editCategorySection.classList.add(
            "hidden"
        );

        categoriesSection.classList.remove(
            "hidden"
        );


        await loadCategories();
    }
);


// =====================================================
// EXHIBIT HELPERS
// =====================================================

async function uploadExhibitCover(
    exhibitId,
    file
) {

    if (!file) {
        return;
    }


    const token =
        localStorage.getItem("token");


    const formData =
        new FormData();


    formData.append(
        "file",
        file
    );


    const response =
        await fetch(
            `${API_BASE_URL}/api/exhibits/${exhibitId}/image`,
            {
                method: "POST",

                headers: {
                    "Authorization":
                        `Bearer ${token}`
                },

                body:
                    formData
            }
        );


    if (!response.ok) {

        const data =
            await response.json();


        throw new Error(
            data.message ||
            "Could not upload exhibit cover image."
        );
    }
}


async function uploadExhibitMedia(
    exhibitId,
    files
) {

    const token =
        localStorage.getItem("token");


    for (const file of files) {

        const formData =
            new FormData();


        formData.append(
            "ExhibitId",
            exhibitId
        );


        formData.append(
            "file",
            file
        );


        const response =
            await fetch(
                `${API_BASE_URL}/api/media`,
                {
                    method: "POST",

                    headers: {
                        "Authorization":
                            `Bearer ${token}`
                    },

                    body:
                        formData
                }
            );


        if (!response.ok) {

            throw new Error(
                `Could not upload ${file.name}.`
            );
        }
    }
}


// =====================================================
// LOAD EXHIBITS
// =====================================================

async function loadExhibitsByCategory(
    categoryId,
    categoryName
) {

    const response =
        await fetch(
            `${API_BASE_URL}/api/exhibits/category/${categoryId}`
        );


    const exhibits =
        await response.json();


    categoriesSection.classList.add("hidden");

    exhibitsSection.classList.remove("hidden");

    createExhibitSection.classList.add("hidden");

    editExhibitSection.classList.add("hidden");

    exhibitDetailsSection.classList.add("hidden");


    selectedCategoryTitle.textContent =
        categoryName;


    exhibitsContainer.innerHTML =
        "";


    exhibits.forEach(exhibit => {

        const card =
            document.createElement("div");

        card.className =
            "exhibit-card";


        if (exhibit.imageUrl) {

            const image =
                document.createElement("img");

            image.className =
                "exhibit-cover";

            image.src =
                exhibit.imageUrl;

            card.appendChild(image);
        }
        else {

            const placeholder =
                document.createElement("div");

            placeholder.className =
                "exhibit-placeholder";

            placeholder.textContent =
                "No cover image";

            card.appendChild(
                placeholder
            );
        }


        const content =
            document.createElement("div");

        content.className =
            "exhibit-content";


        const title =
            document.createElement("h3");

        title.textContent =
            exhibit.title;


        const year =
            document.createElement("p");

        year.textContent =
            `Year: ${exhibit.year}`;


        const visible =
            document.createElement("p");

        visible.textContent =
            `Visible: ${exhibit.visible ? "Yes" : "No"}`;


        const editButton =
            document.createElement("button");

        editButton.className =
            "edit-button";

        editButton.textContent =
            "Edit";


        editButton.addEventListener(
            "click",
            function (event) {

                event.stopPropagation();

                openEditExhibit(exhibit);
            }
        );


        content.appendChild(title);
        content.appendChild(year);
        content.appendChild(visible);
        content.appendChild(editButton);

        card.appendChild(content);


        card.addEventListener(
            "click",
            async function () {

                await showExhibitDetails(
                    exhibit
                );
            }
        );


        exhibitsContainer.appendChild(card);
    });
}


// =====================================================
// CREATE EXHIBIT
// =====================================================

addExhibitButton.addEventListener(
    "click",
    function () {

        exhibitsSection.classList.add("hidden");

        createExhibitSection.classList.remove("hidden");

        createExhibitForm.reset();

        document.getElementById(
            "exhibitVisible"
        ).checked =
            true;
    }
);


createExhibitForm.addEventListener(
    "submit",
    async function (event) {

        event.preventDefault();


        const token =
            localStorage.getItem("token");


        const coverImage =
            document.getElementById(
                "exhibitCoverImage"
            ).files[0];


        const mediaFiles =
            document.getElementById(
                "exhibitMedia"
            ).files;


        try {

            const response =
                await fetch(
                    `${API_BASE_URL}/api/exhibits`,
                    {
                        method: "POST",

                        headers: {
                            "Content-Type":
                                "application/json",

                            "Authorization":
                                `Bearer ${token}`
                        },

                        body: JSON.stringify({

                            title:
                                document.getElementById(
                                    "exhibitTitle"
                                ).value,

                            description:
                                document.getElementById(
                                    "exhibitDescription"
                                ).value,

                            year:
                                parseInt(
                                    document.getElementById(
                                        "exhibitYear"
                                    ).value
                                ),

                            categoryId:
                                currentCategoryId,

                            imagePath:
                                null,

                            visible:
                                document.getElementById(
                                    "exhibitVisible"
                                ).checked
                        })
                    }
                );


            if (!response.ok) {

                throw new Error(
                    "Could not create exhibit."
                );
            }


            const created =
                await response.json();


            await uploadExhibitCover(
                created.id,
                coverImage
            );


            await uploadExhibitMedia(
                created.id,
                mediaFiles
            );


            await loadExhibitsByCategory(
                currentCategoryId,
                currentCategoryName
            );
        }
        catch (error) {

            createExhibitMessage.textContent =
                error.message;
        }
    }
);


// =====================================================
// EDIT EXHIBIT
// =====================================================

function openEditExhibit(exhibit) {

    currentEditingExhibit =
        exhibit;


    exhibitsSection.classList.add("hidden");

    editExhibitSection.classList.remove("hidden");


    document.getElementById(
        "editExhibitTitle"
    ).value =
        exhibit.title;


    document.getElementById(
        "editExhibitDescription"
    ).value =
        exhibit.description;


    document.getElementById(
        "editExhibitYear"
    ).value =
        exhibit.year;


    document.getElementById(
        "editExhibitVisible"
    ).checked =
        exhibit.visible;


    document.getElementById(
        "editExhibitCoverImage"
    ).value =
        "";


    document.getElementById(
        "editExhibitMedia"
    ).value =
        "";
}


editExhibitForm.addEventListener(
    "submit",
    async function (event) {

        event.preventDefault();


        const token =
            localStorage.getItem("token");


        const newCover =
            document.getElementById(
                "editExhibitCoverImage"
            ).files[0];


        const newMedia =
            document.getElementById(
                "editExhibitMedia"
            ).files;


        try {

            const response =
                await fetch(
                    `${API_BASE_URL}/api/exhibits/${currentEditingExhibit.id}`,
                    {
                        method: "PUT",

                        headers: {

                            "Content-Type":
                                "application/json",

                            "Authorization":
                                `Bearer ${token}`
                        },

                        body: JSON.stringify({

                            title:
                                document.getElementById(
                                    "editExhibitTitle"
                                ).value,

                            description:
                                document.getElementById(
                                    "editExhibitDescription"
                                ).value,

                            year:
                                parseInt(
                                    document.getElementById(
                                        "editExhibitYear"
                                    ).value
                                ),

                            categoryId:
                                currentCategoryId,

                            imagePath:
                                currentEditingExhibit.imagePath,

                            visible:
                                document.getElementById(
                                    "editExhibitVisible"
                                ).checked
                        })
                    }
                );


            if (!response.ok) {

                throw new Error(
                    "Could not update exhibit."
                );
            }


            await uploadExhibitCover(
                currentEditingExhibit.id,
                newCover
            );


            await uploadExhibitMedia(
                currentEditingExhibit.id,
                newMedia
            );


            currentEditingExhibit =
                null;


            await loadExhibitsByCategory(
                currentCategoryId,
                currentCategoryName
            );
        }
        catch (error) {

            editExhibitMessage.textContent =
                error.message;
        }
    }
);


// =====================================================
// DELETE EXHIBIT
// =====================================================

deleteExhibitButton.addEventListener(
    "click",
    async function () {

        if (!currentEditingExhibit) {
            return;
        }


        if (!confirm(
            `Delete "${currentEditingExhibit.title}"?`
        )) {
            return;
        }


        const token =
            localStorage.getItem("token");


        const response =
            await fetch(
                `${API_BASE_URL}/api/exhibits/${currentEditingExhibit.id}`,
                {
                    method: "DELETE",

                    headers: {
                        "Authorization":
                            `Bearer ${token}`
                    }
                }
            );


        if (!response.ok) {

            alert(
                "Could not delete exhibit."
            );

            return;
        }


        currentEditingExhibit =
            null;


        await loadExhibitsByCategory(
            currentCategoryId,
            currentCategoryName
        );
    }
);


// =====================================================
// DETAILS
// =====================================================

async function showExhibitDetails(exhibit) {

    exhibitsSection.classList.add("hidden");

    exhibitDetailsSection.classList.remove("hidden");


    exhibitDetailsContainer.innerHTML =
        "";


    if (exhibit.imageUrl) {

        const cover =
            document.createElement("img");

        cover.className =
            "details-cover";

        cover.src =
            exhibit.imageUrl;

        exhibitDetailsContainer.appendChild(
            cover
        );
    }


    const title =
        document.createElement("h1");

    title.textContent =
        exhibit.title;


    const description =
        document.createElement("p");

    description.textContent =
        exhibit.description;


    exhibitDetailsContainer.appendChild(
        title
    );

    exhibitDetailsContainer.appendChild(
        description
    );


    await loadMedia(
        exhibit.id
    );
}


// =====================================================
// MEDIA
// =====================================================

async function loadMedia(exhibitId) {

    const response =
        await fetch(
            `${API_BASE_URL}/api/media/exhibit/${exhibitId}`
        );


    const mediaItems =
        await response.json();


    mediaContainer.innerHTML =
        "";


    mediaItems.forEach(media => {

        const container =
            document.createElement("div");

        container.className =
            "media-item";


        const type =
            (media.fileType || "")
                .toLowerCase();


        if (type.startsWith("image/")) {

            const image =
                document.createElement("img");

            image.src =
                media.url;

            container.appendChild(image);
        }


        else if (type.startsWith("video/")) {

            const video =
                document.createElement("video");

            video.src =
                media.url;

            video.controls =
                true;

            container.appendChild(video);
        }


        else if (type.startsWith("audio/")) {

            const audio =
                document.createElement("audio");

            audio.src =
                media.url;

            audio.controls =
                true;

            container.appendChild(audio);
        }


        mediaContainer.appendChild(
            container
        );
    });
}


// =====================================================
// BACK BUTTONS
// =====================================================

backFromCategoryCreateButton.addEventListener(
    "click",
    function () {

        createCategorySection.classList.add(
            "hidden"
        );

        categoriesSection.classList.remove(
            "hidden"
        );
    }
);


backFromCategoryEditButton.addEventListener(
    "click",
    function () {

        editCategorySection.classList.add(
            "hidden"
        );

        categoriesSection.classList.remove(
            "hidden"
        );
    }
);


backToCategoriesButton.addEventListener(
    "click",
    function () {

        exhibitsSection.classList.add(
            "hidden"
        );

        categoriesSection.classList.remove(
            "hidden"
        );
    }
);


backFromCreateButton.addEventListener(
    "click",
    async function () {

        await loadExhibitsByCategory(
            currentCategoryId,
            currentCategoryName
        );
    }
);


backFromEditButton.addEventListener(
    "click",
    async function () {

        currentEditingExhibit =
            null;

        await loadExhibitsByCategory(
            currentCategoryId,
            currentCategoryName
        );
    }
);


backToExhibitsButton.addEventListener(
    "click",
    async function () {

        await loadExhibitsByCategory(
            currentCategoryId,
            currentCategoryName
        );
    }
);