async function CreateModal(modalId, controller, action, paramId) {

    const response = await fetch(`${controller}/${action}/${paramId}`);
    const modalViewModel = await response.json();

    const bootstrapModal = new bootstrap.Modal(document.getElementById(modalId));

    document.getElementById('labelTitle').innerHTML = modalViewModel.title;
    document.getElementById('labelMessageBody').innerHTML = modalViewModel.message;

    const modalConfirmDeleteButton = document.getElementById('modalConfirmDelete');

    modalConfirmDeleteButton.setAttribute('formaction', `/${modalViewModel.controller}/${modalViewModel.action}/${paramId}`);

    bootstrapModal.show();
}


